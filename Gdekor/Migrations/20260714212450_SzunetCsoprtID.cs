using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gdekor.Migrations
{
    /// <inheritdoc />
    public partial class SzunetCsoprtID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MSzunet_CsoportID",
                table: "MunkaSzunetek_Tbl",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MSzunet_CsoportID",
                table: "MunkaSzunetek_Tbl");
        }
    }
}
