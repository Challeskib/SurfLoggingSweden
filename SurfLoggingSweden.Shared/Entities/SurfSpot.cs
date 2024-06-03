namespace SurfLoggingSweden.Shared.Entities
{
    public class SurfSpot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        // Navigation property for related SurfSessions
        public ICollection<SurfSession> SurfSessions { get; set; } = new List<SurfSession>();
    }
}
