using Microsoft.EntityFrameworkCore;
using SurfLoggingSweden.Shared.Entities;

namespace SurfLoggingSweden.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for SurfSpots
            modelBuilder.Entity<SurfSpot>().HasData(
                new SurfSpot { Id = 1, Name = "Apelviken", Location = "Location A" },
                new SurfSpot { Id = 2, Name = "Skrea Strand", Location = "Location B" },
                new SurfSpot { Id = 3, Name = "Läjet", Location = "Location C" },
                new SurfSpot { Id = 4, Name = "Kåsa", Location = "Location D" }
            );

            // Seed data for SurfSessions
            modelBuilder.Entity<SurfSession>().HasData(
                new SurfSession { Id = 1, SurfSpotId = 1, WindDegree = 190, Rating = 4, WindPower = 15, Start = new DateTime(2024, 5, 1, 8, 0, 0), End = new DateTime(2024, 5, 1, 10, 0, 0) },
                new SurfSession { Id = 2, SurfSpotId = 2, WindDegree = 200, Rating = 3, WindPower = 20, Start = new DateTime(2024, 5, 2, 9, 0, 0), End = new DateTime(2024, 5, 2, 11, 0, 0) },
                new SurfSession { Id = 3, SurfSpotId = 3, WindDegree = 210, Rating = 5, WindPower = 10, Start = new DateTime(2024, 5, 3, 10, 0, 0), End = new DateTime(2024, 5, 3, 12, 0, 0) },
                new SurfSession { Id = 4, SurfSpotId = 4, WindDegree = 220, Rating = 2, WindPower = 25, Start = new DateTime(2024, 5, 4, 7, 0, 0), End = new DateTime(2024, 5, 4, 9, 0, 0) },
                new SurfSession { Id = 5, SurfSpotId = 1, WindDegree = 230, Rating = 1, WindPower = 30, Start = new DateTime(2024, 5, 5, 8, 0, 0), End = new DateTime(2024, 5, 5, 10, 0, 0) },
                new SurfSession { Id = 6, SurfSpotId = 2, WindDegree = 240, Rating = 4, WindPower = 12, Start = new DateTime(2024, 5, 6, 9, 0, 0), End = new DateTime(2024, 5, 6, 11, 0, 0) },
                new SurfSession { Id = 7, SurfSpotId = 3, WindDegree = 250, Rating = 3, WindPower = 18, Start = new DateTime(2024, 5, 7, 10, 0, 0), End = new DateTime(2024, 5, 7, 12, 0, 0) },
                new SurfSession { Id = 8, SurfSpotId = 4, WindDegree = 260, Rating = 5, WindPower = 22, Start = new DateTime(2024, 5, 8, 11, 0, 0), End = new DateTime(2024, 5, 8, 13, 0, 0) }
            );
        }

        public DbSet<SurfSpot> SurfSpots { get; set; }
        public DbSet<SurfSession> SurfSessions { get; set; }
    }
}
