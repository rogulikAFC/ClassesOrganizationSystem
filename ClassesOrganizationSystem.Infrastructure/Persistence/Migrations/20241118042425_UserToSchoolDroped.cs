using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassesOrganizationSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UserToSchoolDroped : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersToSchools");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersToSchools",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    SchoolId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersToSchools", x => new { x.UserId, x.SchoolId });
                    table.ForeignKey(
                        name: "FK_UsersToSchools_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersToSchools_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersToSchools_SchoolId",
                table: "UsersToSchools",
                column: "SchoolId");
        }
    }
}
