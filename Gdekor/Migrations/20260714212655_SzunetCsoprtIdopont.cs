using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gdekor.Migrations
{
    /// <inheritdoc />
    public partial class SzunetCsoprtIdopont : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Letrehoz",
                table: "MunkaSzunetek_Tbl",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Letrehoz",
                table: "MunkaSzunetek_Tbl");
        }
    }
}
