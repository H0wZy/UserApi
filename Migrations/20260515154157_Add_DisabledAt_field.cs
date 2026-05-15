using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace user_api.cs.Migrations
{
    /// <inheritdoc />
    public partial class _20260515154157_Add_DisabledAt_field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "disabled_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "disabled_at",
                table: "users");
        }
    }
}
