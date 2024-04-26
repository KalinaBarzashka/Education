namespace Uc02InventoryControl
{
    // using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using StackExchange.Redis;
    using System.Runtime.CompilerServices;
    using Uc01FacetedSearch;

    public class Program
    {
        // private readonly IConfiguration Configuration;

        // Specify the connection configuration
        public static ConfigurationOptions Options = new ConfigurationOptions
        {
            EndPoints = { "127.0.0.1:6379" }, // Redis server endpoint
            Password = "", // Redis server password
            AbortOnConnectFail = false,
            DefaultDatabase = 0
        };

        public static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
            .AddSingleton<ConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(Options)) // Register Redis connection
            .AddSingleton<IDatabase>(sp => sp.GetRequiredService<ConnectionMultiplexer>().GetDatabase()) // Register Redis database
            .AddSingleton<DataHandler>()
            .BuildServiceProvider();

            // read json events file
            string eventsJsonFilePath = "../../../events.json";
            string eventJsonContent = File.ReadAllText(eventsJsonFilePath);
            var events = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(eventJsonContent)!;

            // read json customers file
            string customersJsonFilePath = "../../../customers.json";
            string customersJsonContent = File.ReadAllText(customersJsonFilePath);
            Customer[] customers = JsonConvert.DeserializeObject<Customer[]>(customersJsonContent)!;

            // flush database before we start
            var connMultiplexer = serviceProvider.GetRequiredService<ConnectionMultiplexer>();
            var server = connMultiplexer.GetServer(hostAndPort: Options.EndPoints.ToString(), asyncState: true);
            await server.FlushDatabaseAsync(0);

            var dataHandler = serviceProvider.GetRequiredService<DataHandler>();

            // create customers
            await dataHandler.CreateCustomers(customers);

            // --------------------------------------------------------------------------------------------- //
            // Test function Check & purchase method
            Console.WriteLine("Create events with 10 tickets available");
            await dataHandler.CreateEvents(events, available: "10");
            Console.WriteLine("== Request 5 tickets, success");
            var requestor = "bill";
            var event_requested = "123-ABC-723";
            await dataHandler.CheckAvailabilityAndPurchase(requestor, event_requested, "5");
        }
    }
}