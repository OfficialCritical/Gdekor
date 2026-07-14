using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gdekor.Migrations
{
    /// <inheritdoc />
    public partial class MnapDatum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MunkaSzunetek_Tbl_Munkanaptok_Tbl_MSzunet_ID",
                table: "MunkaSzunetek_Tbl");

            migrationBuilder.AddColumn<string>(
                name: "M_Datum",
                table: "Munkanaptok_Tbl",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MunkaSzunetek_Tbl_M_Nap_ID",
                table: "MunkaSzunetek_Tbl",
                column: "M_Nap_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_MunkaSzunetek_Tbl_Munkanaptok_Tbl_M_Nap_ID",
                table: "MunkaSzunetek_Tbl",
                column: "M_Nap_ID",
                principalTable: "Munkanaptok_Tbl",
                principalColumn: "MNap_ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MunkaSzunetek_Tbl_Munkanaptok_Tbl_M_Nap_ID",
                table: "MunkaSzunetek_Tbl");

            migrationBuilder.DropIndex(
                name: "IX_MunkaSzunetek_Tbl_M_Nap_ID",
                table: "MunkaSzunetek_Tbl");

            migrationBuilder.DropColumn(
                name: "M_Datum",
                table: "Munkanaptok_Tbl");

            migrationBuilder.AddForeignKey(
                name: "FK_MunkaSzunetek_Tbl_Munkanaptok_Tbl_MSzunet_ID",
                table: "MunkaSzunetek_Tbl",
                column: "MSzunet_ID",
                principalTable: "Munkanaptok_Tbl",
                principalColumn: "MNap_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
