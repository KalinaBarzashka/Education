namespace FindingEventsAndVenues.Models
{
    public class JsonVenue
    {
        public string Venue { get; set; }
        public int Capacity { get; set; }
        public List<Event> Events { get; set; }
        public Geo Geo { get; set; }
        public List<string> Transit { get; set; }
    }
}
