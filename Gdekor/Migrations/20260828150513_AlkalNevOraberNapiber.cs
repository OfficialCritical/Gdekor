using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gdekor.Migrations
{
    /// <inheritdoc />
    public partial class AlkalNevOraberNapiber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Napiber",
                table: "ResztvevokProBen_Tbl",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nev",
                table: "ResztvevokProBen_Tbl",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Oraber",
                table: "ResztvevokProBen_Tbl",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Napiber",
                table: "ResztvevokProBen_Tbl");

            migrationBuilder.DropColumn(
                name: "Nev",
                table: "ResztvevokProBen_Tbl");

            migrationBuilder.DropColumn(
                name: "Oraber",
                table: "ResztvevokProBen_Tbl");
        }
    }
}
