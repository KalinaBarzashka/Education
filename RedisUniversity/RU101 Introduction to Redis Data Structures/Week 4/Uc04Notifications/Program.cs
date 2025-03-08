using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Uc04Notifications
{
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
            // Test function for pub/sub messages for fan out
            Console.WriteLine("== Test 1: Simple pub/sub");

            var events = new string[] { "Womens Judo" };
            foreach (var item in events)
            {
                await dataHandler.CreateEvent(item);
            }
        }
    }
}
