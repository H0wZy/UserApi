using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace user_api.cs.Migrations
{
    /// <inheritdoc />
    public partial class _20260518033922_Add_More_Audit_Fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_online",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "login_method",
                table: "users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_online",
                table: "users");

            migrationBuilder.DropColumn(
                name: "login_method",
                table: "users");
        }
    }
}
