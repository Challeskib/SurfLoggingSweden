namespace SurfLoggingSweden.Shared.Entities
{
    public class SurfSession
    {
        public int Id { get; set; }
        public int SurfSpotId { get; set; }
        public int WindDegree { get; set; } // from 0 to 360 degrees
        public int Rating { get; set; } // from 1 to 5
        public int WindPower { get; set; } // in meters per second
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        // Navigation property for the related SurfSpot
        public SurfSpot? SurfSpot { get; set; }
    }
}
