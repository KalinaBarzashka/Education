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
            DefaultDatabase = 0,
            AllowAdmin = true
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
            var server = connMultiplexer.GetServer(hostAndPort: Options.EndPoints.First().ToString(), asyncState: true);
            await server.FlushDatabaseAsync(0);

            var dataHandler = serviceProvider.GetRequiredService<DataHandler>();

            // create customers
            await dataHandler.CreateCustomers(customers);

            var requestor = "";

            // --------------------------------------------------------------------------------------------- //
            // Test function Check & purchase method
            Console.WriteLine("==Test 1: Check stock levels & purchase");
            Console.WriteLine("Create events with 10 tickets available");
            await dataHandler.CreateEvents(events, available: "10");
            
            Console.WriteLine("== Request 5 tickets, success");
            requestor = "bill";
            var eventRequested = "123-ABC-723";
            await dataHandler.CheckAvailabilityAndPurchase(requestor, eventRequested, "5");
            await dataHandler.PrintEventDitails(eventRequested);
            
            Console.WriteLine("== Request 6 ticket, failure because of insufficient inventory");
            requestor = "mary";
            await dataHandler.CheckAvailabilityAndPurchase(requestor, eventRequested, "6");
            await dataHandler.PrintEventDitails(eventRequested);
        
            // --------------------------------------------------------------------------------------------- //
            Console.WriteLine();
            // Test function reserve & credit auth
            Console.WriteLine("==Test 2: Reserve stock, perform credit auth and complete purchase");
            Console.WriteLine("Create events with 10 tickets available");
            await dataHandler.CreateEvents(events, available: "10");
        
            Console.WriteLine("== Reserve & purchase 5 tickets");
            requestor = "jamie";
            var reserveEventRequested = "737-DEF-911";
            await dataHandler.Reserve(requestor, reserveEventRequested, "5");
            await dataHandler.PrintEventDitails(reserveEventRequested);
        
            Console.WriteLine("== Reserve 5 tickets, failure on auth, return tickets to inventory");
            requestor = "joan";
            await dataHandler.Reserve(requestor, reserveEventRequested, "5");
            await dataHandler.PrintEventDitails(reserveEventRequested);

            // --------------------------------------------------------------------------------------------- //
            Console.WriteLine();
            // Test function expired reservations
            Console.WriteLine("==Test 3: Back out reservations when expiration threshold exceeded");
            Console.WriteLine("Create events");
            await dataHandler.CreateEvents(events);

            // Create expired reservations for the Event
            Console.WriteLine("== Create ticket holds, expire > 30 sec, return tickets to inventory");
            var expiredReserveEventRequested = "320-GHI-921";
            await dataHandler.CreateExpiredReservation(expiredReserveEventRequested);

            await dataHandler.CheckReservations(expiredReserveEventRequested);
        }
    }
}