using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassesOrganizationSistem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRoleRequestAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddRoleRequests",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    SchoolId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddRoleRequests", x => new { x.UserId, x.SchoolId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AddRoleRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AddRoleRequests_SchoolRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "SchoolRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AddRoleRequests_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddRoleRequests_RoleId",
                table: "AddRoleRequests",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AddRoleRequests_SchoolId",
                table: "AddRoleRequests",
                column: "SchoolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddRoleRequests");
        }
    }
}
