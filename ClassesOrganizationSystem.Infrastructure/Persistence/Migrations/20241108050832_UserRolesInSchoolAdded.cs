using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ClassesOrganizationSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UserRolesInSchoolAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SchoolRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersRolesInSchools",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    SchoolId = table.Column<int>(type: "integer", nullable: false),
                    SchoolRoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersRolesInSchools", x => new { x.UserId, x.SchoolId, x.SchoolRoleId });
                    table.ForeignKey(
                        name: "FK_UsersRolesInSchools_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersRolesInSchools_SchoolRoles_SchoolRoleId",
                        column: x => x.SchoolRoleId,
                        principalTable: "SchoolRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersRolesInSchools_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersRolesInSchools_SchoolId",
                table: "UsersRolesInSchools",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersRolesInSchools_SchoolRoleId",
                table: "UsersRolesInSchools",
                column: "SchoolRoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersRolesInSchools");

            migrationBuilder.DropTable(
                name: "SchoolRoles");
        }
    }
}
