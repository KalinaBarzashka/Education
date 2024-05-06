namespace FindingEventsAndVenues
{
    using FindingEventsAndVenues.Models;
    using Newtonsoft.Json;
    using StackExchange.Redis;
    using System.Threading.Tasks;

    public class DataHandler
    {
        private readonly IDatabase db;
        private readonly JsonVenue olympicStadium;
        private readonly JsonVenue nipponBudokan;
        private readonly JsonVenue makuhariMesse;
        private readonly JsonVenue saitamaSuperArena;
        private readonly JsonVenue internationalStadium;
        private readonly JsonVenue isc;

        public DataHandler(IDatabase db)
        {
            this.db = db;
            this.olympicStadium = JsonConvert.DeserializeObject<JsonVenue>(File.ReadAllText("../../../venues/olympic_stadium.json"))!;
            this.nipponBudokan = JsonConvert.DeserializeObject<JsonVenue>(File.ReadAllText("../../../venues/nippon_budokan.json"))!;
            this.makuhariMesse = JsonConvert.DeserializeObject<JsonVenue>(File.ReadAllText("../../../venues/makuhari_messe.json"))!;
            this.saitamaSuperArena = JsonConvert.DeserializeObject<JsonVenue>(File.ReadAllText("../../../venues/saitama_super_arena.json"))!;
            this.internationalStadium = JsonConvert.DeserializeObject<JsonVenue>(File.ReadAllText("../../../venues/international_stadium.json"))!;
            this.isc = JsonConvert.DeserializeObject<JsonVenue>(File.ReadAllText("../../../venues/isc.json"))!;
        }

        public async Task CreateVenues()
        {
            // create each venue
            await this.CreateVenue(olympicStadium);
            await this.CreateVenue(nipponBudokan);
            await this.CreateVenue(makuhariMesse);
            await this.CreateVenue(saitamaSuperArena);
            await this.CreateVenue(internationalStadium);
            await this.CreateVenue(isc);
        }

        public async Task CreateEventLocations()
        {
            // create each event
            await this.CreateEventLocation(olympicStadium);
            await this.CreateEventLocation(nipponBudokan);
            await this.CreateEventLocation(makuhariMesse);
            await this.CreateEventLocation(saitamaSuperArena);
            await this.CreateEventLocation(internationalStadium);
            await this.CreateEventLocation(isc);
        }

        public async Task CreateEventTransitLocations()
        {
            // create each event
            await this.CreateEventTransitLocation(olympicStadium);
            await this.CreateEventTransitLocation(nipponBudokan);
            await this.CreateEventTransitLocation(makuhariMesse);
            await this.CreateEventTransitLocation(saitamaSuperArena);
            await this.CreateEventTransitLocation(internationalStadium);
            await this.CreateEventTransitLocation(isc);
        }

        public async Task GeoRadius(string key, double longitude, double latitude, double radius, GeoUnit unit)
        {
            var res = await this.db.GeoRadiusAsync(key, longitude, latitude, radius, unit, options: GeoRadiusOptions.WithDistance);
            foreach (var item in res)
            {
                Console.WriteLine($"[{item.Member}, {item.Distance}]");
            }
        }

        public async Task GeoRadiusByMember(string key, string member, double radius, GeoUnit unit)
        {
            var res = await this.db.GeoRadiusAsync(key, member, radius, unit, options: GeoRadiusOptions.WithDistance);
            foreach (var item in res)
            {
                Console.WriteLine($"[{item.Member}, {item.Distance}]");
            }
        }

        public async Task GeoDist(string key, string firstMember, string secondMember, GeoUnit unit)
        {
            var res = await this.db.GeoDistanceAsync(key, firstMember, secondMember, unit);
            Console.WriteLine(res.Value);
        }

        private async Task CreateVenue(JsonVenue venue)
        {
            // Create key and geo entry for passed venue
            var key = "geo:venues";
            await this.db.GeoAddAsync(key, venue.Geo.Long, venue.Geo.Lat, venue.Venue);
        }

        private async Task CreateEventLocation(JsonVenue venue)
        {
            // Create geo entry for venues
            var transaction = this.db.CreateTransaction();
            foreach (var item in venue.Events)
            {
                var key = $"geo:events:{item.Name}";
                transaction.GeoAddAsync(key, venue.Geo.Long, venue.Geo.Lat, venue.Venue);
            }

            await transaction.ExecuteAsync();
        }

        private async Task CreateEventTransitLocation(JsonVenue venue)
        {
            // Create geo entries for transit stops for the passed venue
            var transaction = this.db.CreateTransaction();
            foreach (var item in venue.Transit)
            {
                var key = $"geo:transits:{item}";
                transaction.GeoAddAsync(key, venue.Geo.Long, venue.Geo.Lat, venue.Venue);
            }

            await transaction.ExecuteAsync();
        }
    }
}
