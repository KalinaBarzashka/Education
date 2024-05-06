namespace FindingEventsAndVenues
{
    using Microsoft.Extensions.DependencyInjection;
    using StackExchange.Redis;

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

            // flush database before we start
            var connMultiplexer = serviceProvider.GetRequiredService<ConnectionMultiplexer>();
            var server = connMultiplexer.GetServer(hostAndPort: Options.EndPoints.First().ToString(), asyncState: true);
            await server.FlushDatabaseAsync(0);

            var dataHandler = serviceProvider.GetRequiredService<DataHandler>();

            // --------------------------------------------------------------------------------------------- //
            // Test 1 - geo searches around a venue
            Console.WriteLine("==Test 1 - geo searches around a venue");
            await dataHandler.CreateVenues();
            Console.WriteLine("== Find venues with 5km of 'Tokyo Station'");
            var geoKey = "geo:venues";
            await dataHandler.GeoRadius(geoKey, 139.771977, 35.668024, 5, GeoUnit.Kilometers);
            Console.WriteLine("== Find venues within 25km of 'Olympic Stadium'");
            await dataHandler.GeoRadiusByMember(geoKey, "Olympic Stadium", 25, GeoUnit.Kilometers);

            // --------------------------------------------------------------------------------------------- //
            Console.WriteLine();
            // Test 2 - geo searches around events
            Console.WriteLine("==Test 2 - geo searches around events");
            await dataHandler.CreateEventLocations();
            Console.WriteLine("== Find venues for 'Football' within 25km of 'Shin-Yokohama Station'");
            var geoKeyEvent = "geo:events:Football";
            await dataHandler.GeoRadius(geoKeyEvent, 139.606396, 35.509996, 25, GeoUnit.Kilometers);

            // --------------------------------------------------------------------------------------------- //
            Console.WriteLine();
            // Test 3 - geo searched around transit
            Console.WriteLine("==Test 3 - geo searched around transit");
            await dataHandler.CreateEventTransitLocations();
            Console.WriteLine("== Find venues 5km from 'Tokyo Station' on the 'Keiyo Line'");
            var geoKeyTransitEvent = "geo:transits:Keiyo Line";
            await dataHandler.GeoRadius(geoKeyTransitEvent, 139.771977, 35.668024, 5, GeoUnit.Kilometers);
            Console.WriteLine("== Find the distance between 'Makuhari Messe' and 'Tokyo Tatsumi International Swimming Center' on the 'Keiyo Line'");
            await dataHandler.GeoDist(geoKeyTransitEvent, "Makuhari Messe", "Tokyo Tatsumi International Swimming Center", GeoUnit.Kilometers);
            Console.WriteLine("== Find venues within 20km of 'Makuhari Messe' on the 'Keiyo Line'");
            // Note: This only works if the member we are searching for is on the "Keiyo Line". For example, "Olympic Statdium" is not on the "Keiyo Line" so would return zero results.
            await dataHandler.GeoRadiusByMember(geoKeyTransitEvent, "Makuhari Messe", 20, GeoUnit.Kilometers);
        }
    }
}
