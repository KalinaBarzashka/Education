namespace Uc01FacetedSearch
{
    public class Event
    {
        public string Sku { get; set; } = default!;

        public string Name { get; set; } = default!;

        public bool Disabled_access { get; set; }

        public bool Medal_event { get; set; }

        public string Venue { get; set; } = default!;

        public string Category { get; set; } = default!;

        public string Capacity { get; set; } = default!;

        public string Available_general { get; set; } = default!;
    }
}
