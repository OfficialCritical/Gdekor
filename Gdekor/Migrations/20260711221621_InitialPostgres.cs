using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Gdekor.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    SzemelyiSzam = table.Column<string>(type: "text", nullable: true),
                    Nev = table.Column<string>(type: "text", nullable: true),
                    Tel_Korzet = table.Column<string>(type: "text", nullable: true),
                    SzulHely = table.Column<string>(type: "text", nullable: true),
                    SzulIdo = table.Column<string>(type: "text", nullable: true),
                    AnyjaNeve = table.Column<string>(type: "text", nullable: true),
                    ProfKepUtovnal = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projektek_Tbl",
                columns: table => new
                {
                    Pro_ID = table.Column<string>(type: "text", nullable: false),
                    Pro_Nev = table.Column<string>(type: "text", nullable: true),
                    Pro_Allapot = table.Column<string>(type: "text", nullable: true),
                    Pro_Leir = table.Column<string>(type: "text", nullable: true),
                    Pro_KikDolgoznak = table.Column<string>(type: "text", nullable: true),
                    Pro_Terv_Kezdet = table.Column<string>(type: "text", nullable: true),
                    Pro_Terv_Veg = table.Column<string>(type: "text", nullable: true),
                    Pro_Valos_Kezdet = table.Column<string>(type: "text", nullable: true),
                    ProValos_Veg = table.Column<string>(type: "text", nullable: true),
                    Pro_Bevetel = table.Column<string>(type: "text", nullable: true),
                    Pro_Koltseg = table.Column<string>(type: "text", nullable: true),
                    Pro_Profit = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projektek_Tbl", x => x.Pro_ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Munkanaptok_Tbl",
                columns: table => new
                {
                    MNap_ID = table.Column<string>(type: "text", nullable: false),
                    Projekt_ID = table.Column<string>(type: "text", nullable: false),
                    Prof_ID = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Munkanaptok_Tbl", x => x.MNap_ID);
                    table.ForeignKey(
                        name: "FK_Munkanaptok_Tbl_AspNetUsers_Prof_ID",
                        column: x => x.Prof_ID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Munkanaptok_Tbl_Projektek_Tbl_Projekt_ID",
                        column: x => x.Projekt_ID,
                        principalTable: "Projektek_Tbl",
                        principalColumn: "Pro_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResztvevokProBen_Tbl",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Pro_ID = table.Column<string>(type: "text", nullable: true),
                    User_ID = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResztvevokProBen_Tbl", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ResztvevokProBen_Tbl_AspNetUsers_User_ID",
                        column: x => x.User_ID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResztvevokProBen_Tbl_Projektek_Tbl_Pro_ID",
                        column: x => x.Pro_ID,
                        principalTable: "Projektek_Tbl",
                        principalColumn: "Pro_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Munkaorak_Tbl",
                columns: table => new
                {
                    MOra_ID = table.Column<string>(type: "text", nullable: false),
                    M_Nap_ID = table.Column<string>(type: "text", nullable: false),
                    M_Kezdete = table.Column<string>(type: "text", nullable: true),
                    M_Vege = table.Column<string>(type: "text", nullable: true),
                    Letrehoz = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Munkaorak_Tbl", x => x.MOra_ID);
                    table.ForeignKey(
                        name: "FK_Munkaorak_Tbl_Munkanaptok_Tbl_M_Nap_ID",
                        column: x => x.M_Nap_ID,
                        principalTable: "Munkanaptok_Tbl",
                        principalColumn: "MNap_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MunkaSzunetek_Tbl",
                columns: table => new
                {
                    MSzunet_ID = table.Column<string>(type: "text", nullable: false),
                    M_Nap_ID = table.Column<string>(type: "text", nullable: false),
                    Sz_Kezdete = table.Column<string>(type: "text", nullable: true),
                    Sz_Vege = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunkaSzunetek_Tbl", x => x.MSzunet_ID);
                    table.ForeignKey(
                        name: "FK_MunkaSzunetek_Tbl_Munkanaptok_Tbl_MSzunet_ID",
                        column: x => x.MSzunet_ID,
                        principalTable: "Munkanaptok_Tbl",
                        principalColumn: "MNap_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Munkanaptok_Tbl_Prof_ID",
                table: "Munkanaptok_Tbl",
                column: "Prof_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Munkanaptok_Tbl_Projekt_ID",
                table: "Munkanaptok_Tbl",
                column: "Projekt_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Munkaorak_Tbl_M_Nap_ID",
                table: "Munkaorak_Tbl",
                column: "M_Nap_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Munkaorak_Tbl_MOra_ID",
                table: "Munkaorak_Tbl",
                column: "MOra_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MunkaSzunetek_Tbl_MSzunet_ID",
                table: "MunkaSzunetek_Tbl",
                column: "MSzunet_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ResztvevokProBen_Tbl_Pro_ID",
                table: "ResztvevokProBen_Tbl",
                column: "Pro_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ResztvevokProBen_Tbl_User_ID",
                table: "ResztvevokProBen_Tbl",
                column: "User_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Munkaorak_Tbl");

            migrationBuilder.DropTable(
                name: "MunkaSzunetek_Tbl");

            migrationBuilder.DropTable(
                name: "ResztvevokProBen_Tbl");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Munkanaptok_Tbl");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Projektek_Tbl");
        }
    }
}
