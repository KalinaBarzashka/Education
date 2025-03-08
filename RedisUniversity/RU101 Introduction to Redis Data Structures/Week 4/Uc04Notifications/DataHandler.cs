namespace Uc04Notifications
{
    using StackExchange.Redis;
    using System.Threading.Tasks;

    public class DataHandler
    {
        private readonly IDatabase db;

        public DataHandler(IDatabase db)
        {
            this.db = db;
        }

        public async Task CreateEvent(string eventSku)
        {
            // Create the event key from the provided details
            var key = $"event:{eventSku}";
            await this.db.HashSetAsync(key, new HashEntry[]{
                new HashEntry("sku", eventSku)
            });
        }

        public async Task Purchase(string eventSku)
        {
            // Simple purchase function, that pushes the sales order for publishing
            Random rnd = new Random();
            int qty = rnd.Next(1, 10);
            double price = 20;
            var orderId = Guid.NewGuid().ToString();
            var orderInfo = new HashEntry[6];
            orderInfo[0] = new HashEntry("who", "Jim");
            orderInfo[1] = new HashEntry("qty", qty);
            orderInfo[2] = new HashEntry("cost", qty * price);
            orderInfo[3] = new HashEntry("order_id", orderId);
            orderInfo[4] = new HashEntry("event", eventSku);
            orderInfo[5] = new HashEntry("ts", DateTime.Now.ToString());

            await this.PostPurchase(orderId, orderInfo);
        }

        public async Task PostPurchase(string orderId, HashEntry[] orderInfo)
        {
            // Publish purchases to the queue
            var soKey = $"sales_order:{orderId}";
            await this.db.HashSetAsync(soKey, orderInfo);

            var notifyKey = "sales_order_notify";
            await this.db.PublishAsync(notifyKey, orderId);
            notifyKey = $"sales_order_notify:{orderInfo[4].Value}";
            await this.db.PublishAsync(notifyKey, orderId);
        }
    }
}
