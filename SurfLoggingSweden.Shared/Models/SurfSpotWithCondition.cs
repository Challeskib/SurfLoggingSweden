namespace SurfLoggingSweden.Shared.Models
{
    public class SurfSpotWithCondition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int WindDegree { get; set; }
        public double WindSpeedMps { get; set; }
    }
}
