using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ClassesOrganizationSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleRequestIdAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AddRoleRequests",
                table: "AddRoleRequests");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AddRoleRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AddRoleRequests_UserId_RoleId_SchoolId",
                table: "AddRoleRequests",
                columns: new[] { "UserId", "RoleId", "SchoolId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AddRoleRequests",
                table: "AddRoleRequests",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_AddRoleRequests_UserId_RoleId_SchoolId",
                table: "AddRoleRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AddRoleRequests",
                table: "AddRoleRequests");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AddRoleRequests");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AddRoleRequests",
                table: "AddRoleRequests",
                columns: new[] { "UserId", "SchoolId", "RoleId" });
        }
    }
}
