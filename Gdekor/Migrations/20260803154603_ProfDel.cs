using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gdekor.Migrations
{
    /// <inheritdoc />
    public partial class ProfDel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Munkanaptok_Tbl_AspNetUsers_Prof_ID",
                table: "Munkanaptok_Tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_Munkanaptok_Tbl_Projektek_Tbl_Projekt_ID",
                table: "Munkanaptok_Tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_ResztvevokProBen_Tbl_AspNetUsers_User_ID",
                table: "ResztvevokProBen_Tbl");

            migrationBuilder.AddForeignKey(
                name: "FK_Munkanaptok_Tbl_AspNetUsers_Prof_ID",
                table: "Munkanaptok_Tbl",
                column: "Prof_ID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Munkanaptok_Tbl_Projektek_Tbl_Projekt_ID",
                table: "Munkanaptok_Tbl",
                column: "Projekt_ID",
                principalTable: "Projektek_Tbl",
                principalColumn: "Pro_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResztvevokProBen_Tbl_AspNetUsers_User_ID",
                table: "ResztvevokProBen_Tbl",
                column: "User_ID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Munkanaptok_Tbl_AspNetUsers_Prof_ID",
                table: "Munkanaptok_Tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_Munkanaptok_Tbl_Projektek_Tbl_Projekt_ID",
                table: "Munkanaptok_Tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_ResztvevokProBen_Tbl_AspNetUsers_User_ID",
                table: "ResztvevokProBen_Tbl");

            migrationBuilder.AddForeignKey(
                name: "FK_Munkanaptok_Tbl_AspNetUsers_Prof_ID",
                table: "Munkanaptok_Tbl",
                column: "Prof_ID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Munkanaptok_Tbl_Projektek_Tbl_Projekt_ID",
                table: "Munkanaptok_Tbl",
                column: "Projekt_ID",
                principalTable: "Projektek_Tbl",
                principalColumn: "Pro_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResztvevokProBen_Tbl_AspNetUsers_User_ID",
                table: "ResztvevokProBen_Tbl",
                column: "User_ID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
