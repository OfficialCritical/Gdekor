using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gdekor.Migrations
{
    /// <inheritdoc />
    public partial class RenameNapiberToNapiberFtAndAddNapiberOra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Napiber",
                table: "ResztvevokProBen_Tbl",
                newName: "Napiber_Ora");

            migrationBuilder.AddColumn<string>(
                name: "Napiber_Ft",
                table: "ResztvevokProBen_Tbl",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Napiber_Ft",
                table: "ResztvevokProBen_Tbl");

            migrationBuilder.RenameColumn(
                name: "Napiber_Ora",
                table: "ResztvevokProBen_Tbl",
                newName: "Napiber");
        }
    }
}
