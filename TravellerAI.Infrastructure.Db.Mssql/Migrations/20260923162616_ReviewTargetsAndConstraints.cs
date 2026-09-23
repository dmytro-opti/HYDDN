using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravellerAI.Infrastructure.Db.Mssql.Migrations
{
    /// <inheritdoc />
    public partial class ReviewTargetsAndConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Reviews_ReviewId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ReviewId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ReviewId",
                table: "Activities");

            migrationBuilder.AlterColumn<DateTime>(
                name: "JourneyDate",
                table: "UserInfos",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<Guid>(
                name: "ActivityId",
                table: "Reviews",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RoomId",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Transports_SeatCount",
                table: "Transports",
                sql: "[SeatCount] >= 0");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ActivityId",
                table: "Reviews",
                column: "ActivityId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Reviews_Target",
                table: "Reviews",
                sql: "[PlaceId] IS NOT NULL OR [ActivityId] IS NOT NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Bookings_Dates",
                table: "Bookings",
                sql: "[CheckInDate] <= [CheckOutDate]");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Bookings_Guests",
                table: "Bookings",
                sql: "[Adults] >= 0 AND [Children] >= 0");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Activities_ActivityId",
                table: "Reviews",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Activities_ActivityId",
                table: "Reviews");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Transports_SeatCount",
                table: "Transports");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ActivityId",
                table: "Reviews");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Reviews_Target",
                table: "Reviews");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Bookings_Dates",
                table: "Bookings");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Bookings_Guests",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "Reviews");

            migrationBuilder.AlterColumn<DateTime>(
                name: "JourneyDate",
                table: "UserInfos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RoomId",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReviewId",
                table: "Activities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ReviewId",
                table: "Activities",
                column: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Reviews_ReviewId",
                table: "Activities",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id");
        }
    }
}
