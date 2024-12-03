using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassesOrganizationSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedToRoomOnDeleteRoomStatusSetNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomStatuses_StatusId",
                table: "Rooms");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomStatuses_StatusId",
                table: "Rooms",
                column: "StatusId",
                principalTable: "RoomStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomStatuses_StatusId",
                table: "Rooms");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomStatuses_StatusId",
                table: "Rooms",
                column: "StatusId",
                principalTable: "RoomStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
