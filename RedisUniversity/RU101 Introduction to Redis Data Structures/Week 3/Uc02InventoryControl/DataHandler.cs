namespace Uc02InventoryControl
{
    using StackExchange.Redis;
    using System.Data.Common;
    using System.Linq;
    using System.Transactions;

    public class DataHandler
    {
        private readonly IDatabase db;

        public DataHandler(IDatabase db)
        {
            this.db = db;
        }

        public async Task CreateCustomers(Customer[] customers)
        {
            foreach (var item in customers)
            {
                var hashEntries = new HashEntry[]
                {
                    new HashEntry("id", item.Id),
                    new HashEntry("customer_name", item.Customer_Name)
                };

                var key = $"customer:{item.Id}";
                await this.db.HashSetAsync(key, hashEntries);
            }
        }

        public async Task CreateEvents(List<Dictionary<string, string>> events, string available = "None", string price = "None", string tier = "General")
        {
            var eventsSetKey = "events";
            for (int i = 0; i < events.Count(); i++)
            {
                // Override the availability & price if provided
                if (available != "None")
                {
                    events[i][$"available:{tier}"] = available;
                }
                
                if (price != "None")
                {
                    events[i][$"price:{tier}"] = price;
                }

                var eventKey = $"event:{events[i]["sku"]}";

                var hashEntries = new HashEntry[events[i].Count()];
                var counter = 0;
                foreach (var item in events[i])
                {
                    hashEntries[counter] = new HashEntry(item.Key, item.Value);
                    counter++;
                }

                // add the current event and it's data as hash entries into a hash with key event:sku
                await this.db.HashSetAsync(eventKey, hashEntries);
                // the SKU of the event is added to the set that holds all the available events
                await this.db.SetAddAsync(eventsSetKey, events[i]["sku"]);
            }
        }

        public async Task CheckAvailabilityAndPurchase(string customer, string eventSku, string qty, string tier = "General")
        {
            var eventKey = $"event:{eventSku}";
            var quantity = int.Parse(qty);
            // Check if there is sufficient inventory before making the purchase
            try
            {
                var available = int.Parse(await this.db.HashGetAsync(eventKey, (RedisValue)$"available:{tier}"));
                var price = double.Parse(await this.db.HashGetAsync(eventKey, (RedisValue)$"price:{tier}"));
                var eventKeyValue = await this.db.HashGetAsync(eventKey, $"available:{tier}");

                // start a transaction
                var transaction = this.db.CreateTransaction();

                // add condition to watch
                var cond = transaction.AddCondition(Condition.HashEqual(eventKey, $"available:{tier}", eventKeyValue));

                if (available >= quantity)
                {
                    var incrementTask = transaction.HashIncrementAsync(eventKey, (RedisValue)$"available:{tier}", -quantity);
                    var orderId = Guid.NewGuid();
                    var salesOrderKey = $"sales_order:{orderId}";
                    var hashEntries = new HashEntry[7];
                    hashEntries[0] = new HashEntry("order_id", (RedisValue)orderId.ToString());
                    hashEntries[1] = new HashEntry("customer", (RedisValue)customer);
                    hashEntries[2] = new HashEntry("tier", (RedisValue)tier);
                    hashEntries[3] = new HashEntry("qty", (RedisValue)qty);
                    hashEntries[4] = new HashEntry("cost", (RedisValue)(quantity * price));
                    hashEntries[5] = new HashEntry("event_sku", (RedisValue)eventSku);
                    hashEntries[6] = new HashEntry("ts", (RedisValue)DateTime.UtcNow.ToString());
                    var hashSetTask = transaction.HashSetAsync(salesOrderKey, hashEntries);
                }
                else
                {
                    Console.WriteLine($"Insufficient inventory, have {available}, requested {quantity}.");
                    return;
                }

                bool commited = await transaction.ExecuteAsync();

                if (commited)
                {
                    Console.WriteLine("Purchase complete!");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Write Conflict check_availability_and_purchase: {eventKey}. Exception: {ex.Message}");
                return;
            }

            Console.WriteLine("Error during purchase!");
        }

        public async Task Reserve(string customer, string eventSku, string qty, string tier = "General")
        {
            // generate key names for later usage
            var eventKey = $"event:{eventSku}";
            var orderId = Guid.NewGuid();
            var holdKey = $"ticket_hold:{eventSku}";

            // quantity of tickets to be bought
            var quantity = int.Parse(qty);

            // get currently available tickets and price to ticket tier
            var available = int.Parse(await this.db.HashGetAsync(eventKey, (RedisValue)$"available:{tier}"));
            var price = double.Parse(await this.db.HashGetAsync(eventKey, (RedisValue)$"price:{tier}"));

            // first reserve the inventory and perform a credit authorization; if successful then confirm the inventory deduction or back the deducation out
            try
            {
                var eventAvailableValue = await this.db.HashGetAsync(eventKey, $"available:{tier}");
                var eventHeldValue = await this.db.HashGetAsync(eventKey, $"held:{tier}");

                var transaction = this.db.CreateTransaction();
                // add watch command of key
                var cond = transaction.AddCondition(Condition.HashEqual(eventKey, $"available:{tier}", eventAvailableValue));
                var secondCond = transaction.AddCondition(Condition.HashEqual(eventKey, $"held:{tier}", eventHeldValue));

                if (available >= quantity)
                {
                    var incrementAvailableTask = transaction.HashIncrementAsync(eventKey, (RedisValue)$"available:{tier}", -quantity);
                    var incrementHeldTask = transaction.HashIncrementAsync(eventKey, (RedisValue)$"held:{tier}", quantity);
                    
                    // create a hash to store the seat hold information
                    transaction.HashSetAsync(holdKey, $"qty:{orderId}", qty, When.NotExists);
                    transaction.HashSetAsync(holdKey, $"tier:{orderId}", tier, When.NotExists);
                    transaction.HashSetAsync(holdKey, $"ts:{orderId}", DateTime.UtcNow.ToString(), When.NotExists);
                }
                else
                {
                    Console.WriteLine($"Insufficient inventory, have {available}, requested {quantity}.");
                    return;
                }
                bool commited = await transaction.ExecuteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Write Conflict in reserve: {eventKey}. Exception: {ex.Message}");
                return;
            }

            if (this.CreditCardAuth(customer, quantity * price))
            {
                try
                {
                    var eventHeldValue = await this.db.HashGetAsync(eventKey, $"held:{tier}");

                    var purchaseHashEntries = new HashEntry[7];
                    purchaseHashEntries[0] = new HashEntry("order_id", (RedisValue)orderId.ToString());
                    purchaseHashEntries[1] = new HashEntry("customer", (RedisValue)customer);
                    purchaseHashEntries[2] = new HashEntry("tier", (RedisValue)tier);
                    purchaseHashEntries[3] = new HashEntry("qty", (RedisValue)qty);
                    purchaseHashEntries[4] = new HashEntry("cost", (RedisValue)(quantity * price));
                    purchaseHashEntries[5] = new HashEntry("event_sku", (RedisValue)eventSku);
                    purchaseHashEntries[6] = new HashEntry("ts", (RedisValue)DateTime.UtcNow.ToString());

                    var authTransaction = this.db.CreateTransaction();
                    var cond = authTransaction.AddCondition(Condition.HashEqual(eventKey, $"held:{tier}", eventHeldValue));

                    authTransaction.HashDeleteAsync(holdKey, $"qty:{orderId}");
                    authTransaction.HashDeleteAsync(holdKey, $"tier:{orderId}");
                    authTransaction.HashDeleteAsync(holdKey, $"ts:{orderId}");

                    // update the event
                    authTransaction.HashIncrementAsync(eventKey, $"held:{tier}", -quantity);

                    // post the sales order
                    var salesOrderKey = $"sales_order:{orderId}";
                    authTransaction.HashSetAsync(salesOrderKey, purchaseHashEntries);
                    bool commited = await authTransaction.ExecuteAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Write Conflict in reserve: {eventKey}. Exception: {ex.Message}");
                    return;
                }
            }
            else
            {
                Console.WriteLine($"Auth failure on order {orderId} for customer {customer} ${price * quantity}.");
                await this.BackoutHold(eventSku, orderId.ToString());
                return;
            }

            Console.WriteLine("Purchase complete!");
        }

        public async Task CreateExpiredReservation(string eventSku, string tier = "General")
        {
            DateTimeOffset currentTime = DateTime.Now;
            DateTimeOffset unixEpoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
            long currentTimeInSeconds = (long)(currentTime - unixEpoch).TotalSeconds;

            var tickets = new HashEntry[2];
            tickets[0] = new HashEntry($"available:{tier}", 485);
            tickets[1] = new HashEntry($"held:{tier}", 15);

            var holds = new HashEntry[9];
            holds[0] = new HashEntry("qty:VPIR6X", 3);
            holds[1] = new HashEntry($"tier:VPIR6X", tier);
            holds[2] = new HashEntry($"ts:VPIR6X", currentTimeInSeconds - 16);
            holds[3] = new HashEntry("qty:B1BFG7", 5);
            holds[4] = new HashEntry($"tier:B1BFG7", tier);
            holds[5] = new HashEntry($"ts:B1BFG7", currentTimeInSeconds - 22);
            holds[6] = new HashEntry("qty:UZ1EL0", 7);
            holds[7] = new HashEntry($"tier:UZ1EL0", tier);
            holds[8] = new HashEntry($"ts:UZ1EL0", currentTimeInSeconds - 30);

            var key = $"ticket_hold:{eventSku}";
            await this.db.HashSetAsync(key, holds);

            var eventKey = $"event:{eventSku}";
            await this.db.HashSetAsync(eventKey, tickets);
        }

        public async Task CheckReservations(string eventSku)
        {
            var tier = "General";
            var hashKey = $"ticket_hold:{eventSku}";
            var eventKey = $"event:{eventSku}";

            while (true)
            {
                await this.ExpireReservation(eventSku);
                var outstanding = await this.db.HashGetAsync(hashKey, new RedisValue[] { "qty:VPIR6X", "qty:B1BFG7", "qty:UZ1EL0" });
                var available = await this.db.HashGetAsync(eventKey, $"available:{tier}");

                Console.WriteLine($"Event: {eventSku}, Available:{available}, Reservations:{outstanding[0]};{outstanding[1]};{outstanding[2]}");

                // break if all items in outstanding list are None
                if (!outstanding[0].HasValue && !outstanding[1].HasValue && !outstanding[1].HasValue)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        // check if any reservation has exceeded the cutoff time; if any have, then backout the reservation and return the inventory back to the pool
        public async Task ExpireReservation(string eventSku, int cutoffTimeInSeconds = 30)
        {
            DateTimeOffset currentTime = DateTime.Now;
            DateTimeOffset unixEpoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
            long currentCutoffTimeInSeconds = (long)(currentTime - unixEpoch).TotalSeconds - cutoffTimeInSeconds;
            var key = $"ticket_hold:{eventSku}";
            long cursor = 0;
            do
            {
                // constant time complexity O(1)
                var scanResult = this.db.HashScan(key, pattern: "ts:*", pageSize: 1000, cursor: cursor);

                foreach (var item in scanResult)
                {
                    if ((long)item.Value < currentCutoffTimeInSeconds)
                    {
                        var orderId = item.Name.ToString().Split(":")[1];
                        await this.BackoutHold(eventSku, orderId.ToString());
                    }
                }

                cursor = ((IScanningCursor)scanResult).Cursor;
            } while (cursor != 0);
        }

        public async Task PrintEventDitails(string eventSku)
        {
            var eventKey = $"event:{eventSku}";
            var e = await this.db.HashGetAllAsync(eventKey);

            foreach (var item in e)
            {
                Console.WriteLine($"Field name: {item.Name}, field value: {item.Value}");
            }
        }

        private bool CreditCardAuth(string customer, double orderTotal)
        {
            // test function to approve/denigh an authorization request - Always fails Joan's auth
            if (customer.ToUpper() == "JOAN")
            {
                return false;
            }

            return true;
        }

        private async Task BackoutHold(string sku, string orderId)
        {
            var eventKey = $"event:{sku}";
            var holdKey = $"ticket_hold:{sku}";
            try
            {

                var qty = int.Parse(await this.db.HashGetAsync(holdKey, (RedisValue)$"qty:{orderId}"));
                var tier = await this.db.HashGetAsync(holdKey, (RedisValue)$"tier:{orderId}");

                var eventAvailableValue = await this.db.HashGetAsync(eventKey, $"available:{tier}");
                var eventHeldValue = await this.db.HashGetAsync(eventKey, $"held:{tier}");

                var transaction = this.db.CreateTransaction();
                // add watch command of key
                var cond = transaction.AddCondition(Condition.HashEqual(eventKey, $"available:{tier}", eventAvailableValue));
                var secondCond = transaction.AddCondition(Condition.HashEqual(eventKey, $"held:{tier}", eventHeldValue));

                transaction.HashIncrementAsync(eventKey, $"available:{tier}", qty);
                transaction.HashIncrementAsync(eventKey, $"held:{tier}", -qty);

                // remove the hold, since it is no longer needed
                transaction.HashDeleteAsync(holdKey, $"qty:{orderId}");
                transaction.HashDeleteAsync(holdKey, $"tier:{orderId}");
                transaction.HashDeleteAsync(holdKey, $"ts:{orderId}");

                var commited = transaction.ExecuteAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Write Conflict in backout_hold: {eventKey}. Exception: {ex.Message}.");
            }
        }
    }
}
