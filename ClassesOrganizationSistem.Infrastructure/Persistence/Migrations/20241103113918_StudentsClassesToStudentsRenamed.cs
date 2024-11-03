using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassesOrganizationSistem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class StudentsClassesToStudentsRenamed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentClassesToStudents");

            migrationBuilder.CreateTable(
                name: "StudentsClassesToStudents",
                columns: table => new
                {
                    StudentsClassId = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentsClassesToStudents", x => new { x.StudentsClassId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_StudentsClassesToStudents_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentsClassesToStudents_StudentsClass_StudentsClassId",
                        column: x => x.StudentsClassId,
                        principalTable: "StudentsClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentsClassesToStudents_StudentId",
                table: "StudentsClassesToStudents",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentsClassesToStudents");

            migrationBuilder.CreateTable(
                name: "StudentClassesToStudents",
                columns: table => new
                {
                    StudentsClassId = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentClassesToStudents", x => new { x.StudentsClassId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_StudentClassesToStudents_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentClassesToStudents_StudentsClass_StudentsClassId",
                        column: x => x.StudentsClassId,
                        principalTable: "StudentsClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentClassesToStudents_StudentId",
                table: "StudentClassesToStudents",
                column: "StudentId");
        }
    }
}
