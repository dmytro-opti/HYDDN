using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravellerAI.Infrastructure.Db.Mssql.Migrations
{
    /// <inheritdoc />
    public partial class JourneyFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transports_Journeys_JourneyId",
                table: "Transports");

            migrationBuilder.DropForeignKey(
                name: "FK_Transports_Trips_TripId",
                table: "Transports");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Bookings_BookingId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Budgets_BudgetId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Journeys_JourneyId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_BookingId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_BudgetId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_JourneyId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Transports_TripId",
                table: "Transports");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "BudgetId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "JourneyId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Period_End",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Period_Start",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "Transports");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Trips",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "Trips",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Trips",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DistanceKm",
                table: "Trips",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Trips",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "JourneyId",
                table: "Transports",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Journeys",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "Journeys",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "Journeys",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JourneyDays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JourneyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    TripId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JourneyDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JourneyDays_Journeys_JourneyId",
                        column: x => x.JourneyId,
                        principalTable: "Journeys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JourneyDays_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TripStops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TripId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DistanceFromPreviousKm = table.Column<double>(type: "float", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripStops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripStops_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TripStops_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TripStops_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trips_CountryId_City_IsPublic",
                table: "Trips",
                columns: new[] { "CountryId", "City", "IsPublic" });

            migrationBuilder.CreateIndex(
                name: "IX_Journeys_CountryId",
                table: "Journeys",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_JourneyDays_JourneyId_Date",
                table: "JourneyDays",
                columns: new[] { "JourneyId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JourneyDays_TripId",
                table: "JourneyDays",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_TripStops_ActivityId",
                table: "TripStops",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_TripStops_LocationId",
                table: "TripStops",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_TripStops_TripId_Order",
                table: "TripStops",
                columns: new[] { "TripId", "Order" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Journeys_Countries_CountryId",
                table: "Journeys",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transports_Journeys_JourneyId",
                table: "Transports",
                column: "JourneyId",
                principalTable: "Journeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Countries_CountryId",
                table: "Trips",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Journeys_Countries_CountryId",
                table: "Journeys");

            migrationBuilder.DropForeignKey(
                name: "FK_Transports_Journeys_JourneyId",
                table: "Transports");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Countries_CountryId",
                table: "Trips");

            migrationBuilder.DropTable(
                name: "JourneyDays");

            migrationBuilder.DropTable(
                name: "TripStops");

            migrationBuilder.DropIndex(
                name: "IX_Trips_CountryId_City_IsPublic",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Journeys_CountryId",
                table: "Journeys");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "DistanceKm",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Journeys");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "Journeys");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Journeys");

            migrationBuilder.AddColumn<Guid>(
                name: "BookingId",
                table: "Trips",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetId",
                table: "Trips",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "JourneyId",
                table: "Trips",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Period_End",
                table: "Trips",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Period_Start",
                table: "Trips",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Trips",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "JourneyId",
                table: "Transports",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "TripId",
                table: "Transports",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Trips_BookingId",
                table: "Trips",
                column: "BookingId",
                unique: true,
                filter: "[BookingId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_BudgetId",
                table: "Trips",
                column: "BudgetId",
                unique: true,
                filter: "[BudgetId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_JourneyId",
                table: "Trips",
                column: "JourneyId");

            migrationBuilder.CreateIndex(
                name: "IX_Transports_TripId",
                table: "Transports",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transports_Journeys_JourneyId",
                table: "Transports",
                column: "JourneyId",
                principalTable: "Journeys",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transports_Trips_TripId",
                table: "Transports",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Bookings_BookingId",
                table: "Trips",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Budgets_BudgetId",
                table: "Trips",
                column: "BudgetId",
                principalTable: "Budgets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Journeys_JourneyId",
                table: "Trips",
                column: "JourneyId",
                principalTable: "Journeys",
                principalColumn: "Id");
        }
    }
}
