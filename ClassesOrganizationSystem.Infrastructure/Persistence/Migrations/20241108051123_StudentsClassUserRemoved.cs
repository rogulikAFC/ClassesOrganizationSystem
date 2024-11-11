using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassesOrganizationSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class StudentsClassUserRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentsClassUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentsClassUser",
                columns: table => new
                {
                    StudentsClassesId = table.Column<int>(type: "integer", nullable: false),
                    StudentsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentsClassUser", x => new { x.StudentsClassesId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_StudentsClassUser_AspNetUsers_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentsClassUser_StudentsClass_StudentsClassesId",
                        column: x => x.StudentsClassesId,
                        principalTable: "StudentsClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentsClassUser_StudentsId",
                table: "StudentsClassUser",
                column: "StudentsId");
        }
    }
}
