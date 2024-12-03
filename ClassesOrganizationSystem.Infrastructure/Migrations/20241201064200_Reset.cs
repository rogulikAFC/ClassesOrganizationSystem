using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ClassesOrganizationSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Reset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "AspNetRoles",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
            //        NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
            //        ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetRoles", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "RoomStatuses",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        IsOpened = table.Column<bool>(type: "boolean", nullable: false),
            //        Description = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_RoomStatuses", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "SchoolRoles",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        Name = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_SchoolRoles", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Schools",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        Title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
            //        Address = table.Column<string>(type: "text", nullable: true),
            //        Phone = table.Column<string>(type: "text", nullable: false),
            //        Email = table.Column<string>(type: "text", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Schools", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Subjects",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        Title = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Subjects", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetRoleClaims",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        RoleId = table.Column<int>(type: "integer", nullable: false),
            //        ClaimType = table.Column<string>(type: "text", nullable: true),
            //        ClaimValue = table.Column<string>(type: "text", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
            //            column: x => x.RoleId,
            //            principalTable: "AspNetRoles",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Rooms",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        Number = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
            //        Capacity = table.Column<int>(type: "integer", nullable: true),
            //        SchoolId = table.Column<int>(type: "integer", nullable: false),
            //        StatusId = table.Column<int>(type: "integer", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Rooms", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Rooms_RoomStatuses_StatusId",
            //            column: x => x.StatusId,
            //            principalTable: "RoomStatuses",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Rooms_Schools_SchoolId",
            //            column: x => x.SchoolId,
            //            principalTable: "Schools",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "StudentsClasses",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        Title = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
            //        SchoolId = table.Column<int>(type: "integer", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_StudentsClasses", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_StudentsClasses_Schools_SchoolId",
            //            column: x => x.SchoolId,
            //            principalTable: "Schools",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Equipments",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        Title = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
            //        RoomId = table.Column<int>(type: "integer", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Equipments", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Equipments_Rooms_RoomId",
            //            column: x => x.RoomId,
            //            principalTable: "Rooms",
            //            principalColumn: "Id");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUsers",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
            //        Surname = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
            //        Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
            //        PhoneNumber = table.Column<string>(type: "text", nullable: false),
            //        UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
            //        StudentsClassId = table.Column<int>(type: "integer", nullable: true),
            //        NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
            //        NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
            //        EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
            //        PasswordHash = table.Column<string>(type: "text", nullable: true),
            //        SecurityStamp = table.Column<string>(type: "text", nullable: true),
            //        ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
            //        PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
            //        TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
            //        LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
            //        LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
            //        AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_AspNetUsers_StudentsClasses_StudentsClassId",
            //            column: x => x.StudentsClassId,
            //            principalTable: "StudentsClasses",
            //            principalColumn: "Id");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "LessonsSchedules",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        StudentsClassId = table.Column<int>(type: "integer", nullable: false),
            //        DayOfWeek = table.Column<int>(type: "integer", nullable: true),
            //        Date = table.Column<DateOnly>(type: "date", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_LessonsSchedules", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_LessonsSchedules_StudentsClasses_StudentsClassId",
            //            column: x => x.StudentsClassId,
            //            principalTable: "StudentsClasses",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "RoomsToEquipments",
            //    columns: table => new
            //    {
            //        RoomId = table.Column<int>(type: "integer", nullable: false),
            //        EquipmentId = table.Column<int>(type: "integer", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_RoomsToEquipments", x => new { x.RoomId, x.EquipmentId });
            //        table.ForeignKey(
            //            name: "FK_RoomsToEquipments_Equipments_EquipmentId",
            //            column: x => x.EquipmentId,
            //            principalTable: "Equipments",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_RoomsToEquipments_Rooms_RoomId",
            //            column: x => x.RoomId,
            //            principalTable: "Rooms",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AddRoleRequests",
            //    columns: table => new
            //    {
            //        UserId = table.Column<int>(type: "integer", nullable: false),
            //        SchoolId = table.Column<int>(type: "integer", nullable: false),
            //        RoleId = table.Column<int>(type: "integer", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AddRoleRequests", x => new { x.UserId, x.SchoolId, x.RoleId });
            //        table.ForeignKey(
            //            name: "FK_AddRoleRequests_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_AddRoleRequests_SchoolRoles_RoleId",
            //            column: x => x.RoleId,
            //            principalTable: "SchoolRoles",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_AddRoleRequests_Schools_SchoolId",
            //            column: x => x.SchoolId,
            //            principalTable: "Schools",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserClaims",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        UserId = table.Column<int>(type: "integer", nullable: false),
            //        ClaimType = table.Column<string>(type: "text", nullable: true),
            //        ClaimValue = table.Column<string>(type: "text", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_AspNetUserClaims_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserLogins",
            //    columns: table => new
            //    {
            //        LoginProvider = table.Column<string>(type: "text", nullable: false),
            //        ProviderKey = table.Column<string>(type: "text", nullable: false),
            //        ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
            //        UserId = table.Column<int>(type: "integer", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
            //        table.ForeignKey(
            //            name: "FK_AspNetUserLogins_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserRoles",
            //    columns: table => new
            //    {
            //        UserId = table.Column<int>(type: "integer", nullable: false),
            //        RoleId = table.Column<int>(type: "integer", nullable: false),
            //        Discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
            //        table.ForeignKey(
            //            name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
            //            column: x => x.RoleId,
            //            principalTable: "AspNetRoles",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_AspNetUserRoles_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserTokens",
            //    columns: table => new
            //    {
            //        UserId = table.Column<int>(type: "integer", nullable: false),
            //        LoginProvider = table.Column<string>(type: "text", nullable: false),
            //        Name = table.Column<string>(type: "text", nullable: false),
            //        Value = table.Column<string>(type: "text", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
            //        table.ForeignKey(
            //            name: "FK_AspNetUserTokens_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "RoleUser",
            //    columns: table => new
            //    {
            //        RolesId = table.Column<int>(type: "integer", nullable: false),
            //        UsersId = table.Column<int>(type: "integer", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_RoleUser", x => new { x.RolesId, x.UsersId });
            //        table.ForeignKey(
            //            name: "FK_RoleUser_AspNetRoles_RolesId",
            //            column: x => x.RolesId,
            //            principalTable: "AspNetRoles",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_RoleUser_AspNetUsers_UsersId",
            //            column: x => x.UsersId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "StudentsClassesToStudents",
            //    columns: table => new
            //    {
            //        StudentsClassId = table.Column<int>(type: "integer", nullable: false),
            //        StudentId = table.Column<int>(type: "integer", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_StudentsClassesToStudents", x => new { x.StudentsClassId, x.StudentId });
            //        table.ForeignKey(
            //            name: "FK_StudentsClassesToStudents_AspNetUsers_StudentId",
            //            column: x => x.StudentId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_StudentsClassesToStudents_StudentsClasses_StudentsClassId",
            //            column: x => x.StudentsClassId,
            //            principalTable: "StudentsClasses",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "UsersRolesInSchools",
            //    columns: table => new
            //    {
            //        UserId = table.Column<int>(type: "integer", nullable: false),
            //        SchoolId = table.Column<int>(type: "integer", nullable: false),
            //        SchoolRoleId = table.Column<int>(type: "integer", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_UsersRolesInSchools", x => new { x.UserId, x.SchoolId, x.SchoolRoleId });
            //        table.ForeignKey(
            //            name: "FK_UsersRolesInSchools_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_UsersRolesInSchools_SchoolRoles_SchoolRoleId",
            //            column: x => x.SchoolRoleId,
            //            principalTable: "SchoolRoles",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_UsersRolesInSchools_Schools_SchoolId",
            //            column: x => x.SchoolId,
            //            principalTable: "Schools",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Lessons",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        SerialNumber = table.Column<int>(type: "integer", nullable: false),
            //        TeacherId = table.Column<int>(type: "integer", nullable: false),
            //        RoomId = table.Column<int>(type: "integer", nullable: false),
            //        SubjectId = table.Column<int>(type: "integer", nullable: false),
            //        LessonsScheduleId = table.Column<int>(type: "integer", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Lessons", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Lessons_AspNetUsers_TeacherId",
            //            column: x => x.TeacherId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Lessons_LessonsSchedules_LessonsScheduleId",
            //            column: x => x.LessonsScheduleId,
            //            principalTable: "LessonsSchedules",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Lessons_Rooms_RoomId",
            //            column: x => x.RoomId,
            //            principalTable: "Rooms",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Lessons_Subjects_SubjectId",
            //            column: x => x.SubjectId,
            //            principalTable: "Subjects",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_AddRoleRequests_RoleId",
            //    table: "AddRoleRequests",
            //    column: "RoleId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AddRoleRequests_SchoolId",
            //    table: "AddRoleRequests",
            //    column: "SchoolId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetRoleClaims_RoleId",
            //    table: "AspNetRoleClaims",
            //    column: "RoleId");

            //migrationBuilder.CreateIndex(
            //    name: "RoleNameIndex",
            //    table: "AspNetRoles",
            //    column: "NormalizedName",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUserClaims_UserId",
            //    table: "AspNetUserClaims",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUserLogins_UserId",
            //    table: "AspNetUserLogins",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUserRoles_RoleId",
            //    table: "AspNetUserRoles",
            //    column: "RoleId");

            //migrationBuilder.CreateIndex(
            //    name: "EmailIndex",
            //    table: "AspNetUsers",
            //    column: "NormalizedEmail");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUsers_StudentsClassId",
            //    table: "AspNetUsers",
            //    column: "StudentsClassId");

            //migrationBuilder.CreateIndex(
            //    name: "UserNameIndex",
            //    table: "AspNetUsers",
            //    column: "NormalizedUserName",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Equipments_RoomId",
            //    table: "Equipments",
            //    column: "RoomId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Lessons_LessonsScheduleId",
            //    table: "Lessons",
            //    column: "LessonsScheduleId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Lessons_RoomId",
            //    table: "Lessons",
            //    column: "RoomId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Lessons_SubjectId",
            //    table: "Lessons",
            //    column: "SubjectId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Lessons_TeacherId",
            //    table: "Lessons",
            //    column: "TeacherId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_LessonsSchedules_StudentsClassId",
            //    table: "LessonsSchedules",
            //    column: "StudentsClassId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_RoleUser_UsersId",
            //    table: "RoleUser",
            //    column: "UsersId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Rooms_SchoolId",
            //    table: "Rooms",
            //    column: "SchoolId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Rooms_StatusId",
            //    table: "Rooms",
            //    column: "StatusId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_RoomsToEquipments_EquipmentId",
            //    table: "RoomsToEquipments",
            //    column: "EquipmentId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_StudentsClasses_SchoolId",
            //    table: "StudentsClasses",
            //    column: "SchoolId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_StudentsClassesToStudents_StudentId",
            //    table: "StudentsClassesToStudents",
            //    column: "StudentId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_UsersRolesInSchools_SchoolId",
            //    table: "UsersRolesInSchools",
            //    column: "SchoolId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_UsersRolesInSchools_SchoolRoleId",
            //    table: "UsersRolesInSchools",
            //    column: "SchoolRoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddRoleRequests");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropTable(
                name: "RoomsToEquipments");

            migrationBuilder.DropTable(
                name: "StudentsClassesToStudents");

            migrationBuilder.DropTable(
                name: "UsersRolesInSchools");

            migrationBuilder.DropTable(
                name: "LessonsSchedules");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SchoolRoles");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "StudentsClasses");

            migrationBuilder.DropTable(
                name: "RoomStatuses");

            migrationBuilder.DropTable(
                name: "Schools");
        }
    }
}
