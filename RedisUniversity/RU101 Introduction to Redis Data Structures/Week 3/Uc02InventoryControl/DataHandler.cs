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
