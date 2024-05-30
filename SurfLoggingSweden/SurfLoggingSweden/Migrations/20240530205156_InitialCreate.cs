using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurfLoggingSweden.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SurfSpots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurfSpots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SurfSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurfSpotId = table.Column<int>(type: "int", nullable: false),
                    WindDegree = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    WindPower = table.Column<int>(type: "int", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurfSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurfSessions_SurfSpots_SurfSpotId",
                        column: x => x.SurfSpotId,
                        principalTable: "SurfSpots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "SurfSpots",
                columns: new[] { "Id", "Location", "Name" },
                values: new object[,]
                {
                    { 1, "Location A", "Apelviken" },
                    { 2, "Location B", "Skrea Strand" },
                    { 3, "Location C", "Läjet" },
                    { 4, "Location D", "Kåsa" }
                });

            migrationBuilder.InsertData(
                table: "SurfSessions",
                columns: new[] { "Id", "End", "Rating", "Start", "SurfSpotId", "WindDegree", "WindPower" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 5, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(2024, 5, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), 1, 190, 15 },
                    { 2, new DateTime(2024, 5, 2, 11, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2024, 5, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), 2, 200, 20 },
                    { 3, new DateTime(2024, 5, 3, 12, 0, 0, 0, DateTimeKind.Unspecified), 5, new DateTime(2024, 5, 3, 10, 0, 0, 0, DateTimeKind.Unspecified), 3, 210, 10 },
                    { 4, new DateTime(2024, 5, 4, 9, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2024, 5, 4, 7, 0, 0, 0, DateTimeKind.Unspecified), 4, 220, 25 },
                    { 5, new DateTime(2024, 5, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2024, 5, 5, 8, 0, 0, 0, DateTimeKind.Unspecified), 1, 230, 30 },
                    { 6, new DateTime(2024, 5, 6, 11, 0, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(2024, 5, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), 2, 240, 12 },
                    { 7, new DateTime(2024, 5, 7, 12, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2024, 5, 7, 10, 0, 0, 0, DateTimeKind.Unspecified), 3, 250, 18 },
                    { 8, new DateTime(2024, 5, 8, 13, 0, 0, 0, DateTimeKind.Unspecified), 5, new DateTime(2024, 5, 8, 11, 0, 0, 0, DateTimeKind.Unspecified), 4, 260, 22 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SurfSessions_SurfSpotId",
                table: "SurfSessions",
                column: "SurfSpotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SurfSessions");

            migrationBuilder.DropTable(
                name: "SurfSpots");
        }
    }
}
