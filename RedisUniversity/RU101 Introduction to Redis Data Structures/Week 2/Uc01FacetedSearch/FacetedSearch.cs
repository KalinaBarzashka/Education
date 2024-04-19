namespace Uc01FacetedSearch
{
    using Newtonsoft.Json;
    using StackExchange.Redis;
    using System.Reflection;

    public class FacetedSearch
    {
        private readonly IDatabase db;
        private readonly string[] lookupAttrs = new string[3] { "disabled_access", "medal_event", "venue" };

        public FacetedSearch(IDatabase db)
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

                foreach (var item in lookupAttrs)
                {
                    PropertyInfo property = typeof(Event).GetProperty(item, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)!;
                    if (property != null)
                    {
                        object entValue = property.GetValue(evt)!;

                        if (entValue != null)
                        {
                            var fsKey = $"fs:{item}:{entValue}";
                            await this.db.SetAddAsync(fsKey, evt.Sku);
                        }
                    }
                }
            }
        }

        public async Task MatchByFaceting(IDictionary<string, string> matchingAttributes)
        {
            var matches = new List<RedisKey>();

            foreach (var attr in matchingAttributes)
            {
                RedisKey fsKey = $"fs:{attr.Key}:{attr.Value}";
                matches.Add(fsKey);
            }

            var result = await this.db.SetCombineAsync(SetOperation.Intersect, matches.ToArray());

            foreach (var item in result)
            {
                string matchKey = $"event:{item}";
                Console.WriteLine(await db.StringGetAsync(matchKey));
            }
        }
    }
}
