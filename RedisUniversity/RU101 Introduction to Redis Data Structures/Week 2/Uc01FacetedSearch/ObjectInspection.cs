namespace Uc01FacetedSearch
{
    using Newtonsoft.Json;
    using StackExchange.Redis;
    using System.Reflection;

    public class ObjectInspection
    {
        private readonly IDatabase db;

        public ObjectInspection(IDatabase db)
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
            }
        }

        public async Task MatchByInspection(IDictionary<string, string> matchingAttributes)
        {
            var matches = new List<Event>();
            var matchingPattern = "event:99*";
            long cursor = 0;

            do
            {
                var scanResults = await db.ExecuteAsync("scan", cursor, "MATCH", matchingPattern, "COUNT", 1000);
                cursor = (long)scanResults[0];
                var keys = (RedisKey[])scanResults[1]!;

                if (keys == null || keys.Length == 0)
                {
                    continue;
                }

                foreach (var curKey in keys)
                {
                    var evn = await db.StringGetAsync(curKey);
                    Event eventObject = JsonConvert.DeserializeObject<Event>(evn);

                    if (eventObject != null)
                    {
                        bool isMatch = true;
                        foreach (var attr in matchingAttributes)
                        {
                            PropertyInfo property = typeof(Event).GetProperty(attr.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                            if (property != null)
                            {
                                object value = property.GetValue(eventObject);

                                if(!value.ToString().Equals(attr.Value, StringComparison.OrdinalIgnoreCase))
                                {
                                    isMatch = false;
                                    break;
                                }
                            }
                        }

                        if (isMatch)
                        {
                            matches.Add(eventObject);
                        }
                    }
                }
            }
            while (cursor != 0);


            foreach (var match in matches)
            {
                string matchKey = "event:" + match.Sku;
                Console.WriteLine(await db.StringGetAsync(matchKey));
            }
        }
    }
}
