using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gdekor.Migrations
{
    /// <inheritdoc />
    public partial class NewMunkaOraim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Munkaorak_Tbl_Munkanaptok_Tbl_M_Nap_ID",
                table: "Munkaorak_Tbl");

            migrationBuilder.RenameColumn(
                name: "M_Nap_ID",
                table: "Munkaorak_Tbl",
                newName: "M_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Munkaorak_Tbl_M_Nap_ID",
                table: "Munkaorak_Tbl",
                newName: "IX_Munkaorak_Tbl_M_UserID");

            migrationBuilder.AddColumn<string>(
                name: "M_Nap",
                table: "Munkaorak_Tbl",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "M_ProID",
                table: "Munkaorak_Tbl",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Munkaorak_Tbl_M_ProID",
                table: "Munkaorak_Tbl",
                column: "M_ProID");

            migrationBuilder.AddForeignKey(
                name: "FK_Munkaorak_Tbl_AspNetUsers_M_UserID",
                table: "Munkaorak_Tbl",
                column: "M_UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Munkaorak_Tbl_Projektek_Tbl_M_ProID",
                table: "Munkaorak_Tbl",
                column: "M_ProID",
                principalTable: "Projektek_Tbl",
                principalColumn: "Pro_ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Munkaorak_Tbl_AspNetUsers_M_UserID",
                table: "Munkaorak_Tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_Munkaorak_Tbl_Projektek_Tbl_M_ProID",
                table: "Munkaorak_Tbl");

            migrationBuilder.DropIndex(
                name: "IX_Munkaorak_Tbl_M_ProID",
                table: "Munkaorak_Tbl");

            migrationBuilder.DropColumn(
                name: "M_Nap",
                table: "Munkaorak_Tbl");

            migrationBuilder.DropColumn(
                name: "M_ProID",
                table: "Munkaorak_Tbl");

            migrationBuilder.RenameColumn(
                name: "M_UserID",
                table: "Munkaorak_Tbl",
                newName: "M_Nap_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Munkaorak_Tbl_M_UserID",
                table: "Munkaorak_Tbl",
                newName: "IX_Munkaorak_Tbl_M_Nap_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Munkaorak_Tbl_Munkanaptok_Tbl_M_Nap_ID",
                table: "Munkaorak_Tbl",
                column: "M_Nap_ID",
                principalTable: "Munkanaptok_Tbl",
                principalColumn: "MNap_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
