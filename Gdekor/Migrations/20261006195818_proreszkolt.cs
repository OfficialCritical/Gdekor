using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gdekor.Migrations
{
    /// <inheritdoc />
    public partial class proreszkolt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Pro_Egyeb_reszKolt",
                table: "Projektek_Tbl",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pro_Egyeb_reszKolt",
                table: "Projektek_Tbl");
        }
    }
}
