namespace Uc02InventoryControl
{
    using StackExchange.Redis;
    using System.Linq;

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
                foreach (var item in events[i])
                {
                    hashEntries.Append(new HashEntry(item.Key, item.Value));
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
                // add watch command of key
                var transaction = this.db.CreateTransaction();
                var available = int.Parse(await transaction.HashGetAsync(eventKey, (RedisValue)$"available:{tier}"));
                var price = Double.Parse(await transaction.HashGetAsync(eventKey, (RedisValue)$"price:{tier}"));
                if (available >= quantity)
                {
                    await transaction.HashIncrementAsync(eventKey, (RedisValue)$"available:{tier}", -quantity);
                    var orderId = Guid.NewGuid();
                    var salesOrderKey = $"sales_order:{orderId}";
                    var hashEntries = new HashEntry[7];
                    hashEntries.Append(new HashEntry("order_id", (RedisValue)orderId.ToString()));
                    hashEntries.Append(new HashEntry("customer", (RedisValue)customer));
                    hashEntries.Append(new HashEntry("tier", (RedisValue)tier));
                    hashEntries.Append(new HashEntry("qty", (RedisValue)qty));
                    hashEntries.Append(new HashEntry("cost", (RedisValue)(quantity * price)));
                    hashEntries.Append(new HashEntry("event_sku", (RedisValue)eventSku));
                    hashEntries.Append(new HashEntry("ts", (RedisValue)DateTime.UtcNow.ToString()));
                    await transaction.HashSetAsync(salesOrderKey, hashEntries);
                }
                else
                {
                    Console.WriteLine($"Insufficient inventory, have {available}, requested {quantity}.");
                }
                bool commited = await transaction.ExecuteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Write Conflict check_availability_and_purchase: {eventKey}. Exception: {ex.Message}");
                return;
            }
            Console.WriteLine("Purchase complete!");
        }
    }
}
