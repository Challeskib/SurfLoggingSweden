namespace SurfLoggingSweden.Shared.Models
{
    public class SurfSessionDto
    {
        public int Id { get; set; }
        public int SurfSpotId { get; set; }
        public string SurfSpotName { get; set; }
        public int WindDegree { get; set; }
        public int Rating { get; set; }
        public int WindPower { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
