using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace user_api.cs.Migrations
{
    /// <inheritdoc />
    public partial class _20260516183502_Remove_AcceptedTermsAt_Default : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                     ALTER TABLE users
                                     ALTER COLUMN accepted_terms_at DROP DEFAULT;
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
