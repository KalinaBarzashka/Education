using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StackExchange.Redis;
using Uc01FacetedSearch;

public class Program
{
    private readonly IConfiguration Configuration;

    public static async Task Main(string[] args)
    {
        try
        {

            var serviceProvider = new ServiceCollection()
            .AddSingleton<ConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect("TODO")) // Register Redis connection
            .AddSingleton<IDatabase>(sp => sp.GetRequiredService<ConnectionMultiplexer>().GetDatabase()) // Register Redis database
            .AddSingleton<ObjectInspection>()
            .BuildServiceProvider();

            // read json events file
            string jsonFilePath = "../../../events.json";
            string jsonContent = File.ReadAllText(jsonFilePath);
            Event[] events = JsonConvert.DeserializeObject<Event[]>(jsonContent);

            // Method 1: Object Inspection
            var objInspection = serviceProvider.GetRequiredService<ObjectInspection>();

            // Load events
            await objInspection.CreateEvents(events);

            // Find matches
            Console.WriteLine("=== disabled_access=True");
            await objInspection.MatchByInspection(new Dictionary<string, string> { { "disabled_access", "true" } });
            
            Console.WriteLine("=== disabled_access=True, medal_event=False");
            await objInspection.MatchByInspection(new Dictionary<string, string> { { "disabled_access", "true" }, { "medal_event", "false" } });

            Console.WriteLine("=== disabled_access=False, medal_event=False, venue='Nippon Budokan'");
            await objInspection.MatchByInspection(new Dictionary<string, string> { { "disabled_access", "false" }, { "medal_event", "false" }, { "venue", "Nippon Budokan" } });

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
