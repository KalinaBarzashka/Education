namespace Uc01FacetedSearch
{
    using Newtonsoft.Json;
    using StackExchange.Redis;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;

    public class HashedKeys
    {
        private readonly IDatabase db;
        private readonly string[] lookupAttrs = new string[3] { "disabled_access", "medal_event", "venue" };

        public HashedKeys(IDatabase db)
        {
            this.db = db;
        }

        public async Task CreateEvents(Event[] events)
        {
            foreach (var evt in events)
            {
                string key = $"event:{evt.Sku}";
                string value = JsonConvert.SerializeObject(evt);
                await db.StringSetAsync(key, value);

                var hfs = new Dictionary<string, string>();

                foreach (var item in lookupAttrs)
                {
                    PropertyInfo property = typeof(Event).GetProperty(item, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)!;
                    if (property != null)
                    {
                        object entValue = property.GetValue(evt)!;

                        if (entValue != null)
                        {
                            hfs.Add(item, entValue.ToString());

                            var strBuilder = new StringBuilder();
                            foreach (var kvp in hfs)
                            {
                                strBuilder.Append($"{kvp.Key}:{kvp.Value},");
                            }
                            strBuilder.Remove(strBuilder.Length - 1, 1);
                            var hashedValue = this.GetHashValue(strBuilder.ToString());
                            var hfsKey = $"hfs:{hashedValue}";
                            await this.db.SetAddAsync(hfsKey, $"event:{evt.Sku}");
                        }
                    }
                }
            }
        }

        public async Task MatchByHashedKeys(IDictionary<string, string> matchingAttributes)
        {
            List<string> matches = new List<string>();
            var hfs = new Dictionary<string, string>();

            foreach (var item in matchingAttributes)
            {
                if (lookupAttrs.Contains(item.Key))
                {
                    hfs[item.Key] = item.Value;
                }
            }

            string concatenatedKeys = string.Join(",", hfs.Select(x => x.Key + ":" + x.Value));

            var hashedValue = this.GetHashValue(concatenatedKeys);
            string hashedKey = $"hfs:{hashedValue}";

            long cursor = 0;
            do
            {
                var scanResults = db.SetScan(hashedKey, pageSize: 100);
                cursor = ((IScanningCursor)scanResults).Cursor;
                var keys = scanResults.ToArray();
                
                if (keys == null || keys.Length == 0)
                {
                    continue;
                }
                
                foreach (var curKey in keys)
                {
                    Console.WriteLine(await db.StringGetAsync(curKey.ToString()));
                }
            }
            while (cursor != 0);
        }

        private string GetHashValue(string key)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(key);

            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash value of the input bytes
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // Convert the byte array to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
