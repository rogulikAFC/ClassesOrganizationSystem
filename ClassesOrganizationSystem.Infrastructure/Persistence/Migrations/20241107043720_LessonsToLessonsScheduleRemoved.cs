using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassesOrganizationSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class LessonsToLessonsScheduleRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessonsToLessonsSchedules");

            migrationBuilder.AddColumn<int>(
                name: "LessonsScheduleId",
                table: "Lessons",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Equipments",
                type: "integer",
                nullable: true);

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
                name: "IX_StudentsClass_SchoolId",
                table: "StudentsClass",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_LessonsScheduleId",
                table: "Lessons",
                column: "LessonsScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_RoomId",
                table: "Equipments",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsClassUser_StudentsId",
                table: "StudentsClassUser",
                column: "StudentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipments_Rooms_RoomId",
                table: "Equipments",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_LessonsSchedules_LessonsScheduleId",
                table: "Lessons",
                column: "LessonsScheduleId",
                principalTable: "LessonsSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsClass_Schools_SchoolId",
                table: "StudentsClass",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipments_Rooms_RoomId",
                table: "Equipments");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_LessonsSchedules_LessonsScheduleId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsClass_Schools_SchoolId",
                table: "StudentsClass");

            migrationBuilder.DropTable(
                name: "StudentsClassUser");

            migrationBuilder.DropIndex(
                name: "IX_StudentsClass_SchoolId",
                table: "StudentsClass");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_LessonsScheduleId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Equipments_RoomId",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "LessonsScheduleId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Equipments");

            migrationBuilder.CreateTable(
                name: "LessonsToLessonsSchedules",
                columns: table => new
                {
                    LessonId = table.Column<int>(type: "integer", nullable: false),
                    LessonsScheduleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonsToLessonsSchedules", x => new { x.LessonId, x.LessonsScheduleId });
                    table.ForeignKey(
                        name: "FK_LessonsToLessonsSchedules_LessonsSchedules_LessonsScheduleId",
                        column: x => x.LessonsScheduleId,
                        principalTable: "LessonsSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonsToLessonsSchedules_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonsToLessonsSchedules_LessonsScheduleId",
                table: "LessonsToLessonsSchedules",
                column: "LessonsScheduleId");
        }
    }
}
