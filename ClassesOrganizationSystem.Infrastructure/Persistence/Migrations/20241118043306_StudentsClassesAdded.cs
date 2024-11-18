using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassesOrganizationSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class StudentsClassesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonsSchedules_StudentsClass_StudentsClassId",
                table: "LessonsSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsClass_Schools_SchoolId",
                table: "StudentsClass");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsClassesToStudents_StudentsClass_StudentsClassId",
                table: "StudentsClassesToStudents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentsClass",
                table: "StudentsClass");

            migrationBuilder.RenameTable(
                name: "StudentsClass",
                newName: "StudentsClasses");

            migrationBuilder.RenameIndex(
                name: "IX_StudentsClass_SchoolId",
                table: "StudentsClasses",
                newName: "IX_StudentsClasses_SchoolId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentsClasses",
                table: "StudentsClasses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonsSchedules_StudentsClasses_StudentsClassId",
                table: "LessonsSchedules",
                column: "StudentsClassId",
                principalTable: "StudentsClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsClasses_Schools_SchoolId",
                table: "StudentsClasses",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsClassesToStudents_StudentsClasses_StudentsClassId",
                table: "StudentsClassesToStudents",
                column: "StudentsClassId",
                principalTable: "StudentsClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonsSchedules_StudentsClasses_StudentsClassId",
                table: "LessonsSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsClasses_Schools_SchoolId",
                table: "StudentsClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsClassesToStudents_StudentsClasses_StudentsClassId",
                table: "StudentsClassesToStudents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentsClasses",
                table: "StudentsClasses");

            migrationBuilder.RenameTable(
                name: "StudentsClasses",
                newName: "StudentsClass");

            migrationBuilder.RenameIndex(
                name: "IX_StudentsClasses_SchoolId",
                table: "StudentsClass",
                newName: "IX_StudentsClass_SchoolId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentsClass",
                table: "StudentsClass",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonsSchedules_StudentsClass_StudentsClassId",
                table: "LessonsSchedules",
                column: "StudentsClassId",
                principalTable: "StudentsClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsClass_Schools_SchoolId",
                table: "StudentsClass",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsClassesToStudents_StudentsClass_StudentsClassId",
                table: "StudentsClassesToStudents",
                column: "StudentsClassId",
                principalTable: "StudentsClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
