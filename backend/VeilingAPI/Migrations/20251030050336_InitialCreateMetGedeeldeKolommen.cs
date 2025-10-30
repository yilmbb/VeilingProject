using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeilingAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateMetGedeeldeKolommen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gebruikers",
                columns: table => new
                {
                    gebruiker_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    wachtwoord = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    registreren = table.Column<DateTime>(type: "datetime2", nullable: false),
                    inloggen = table.Column<DateTime>(type: "datetime2", nullable: true),
                    gebruiker_type = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    bedrijfsnaam = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    kvk = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    btw_nummer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    telefoon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    leveradres = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    rekening_nummer = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: true),
                    bedrijfsadres = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gebruikers", x => x.gebruiker_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gebruikers_email",
                table: "Gebruikers",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gebruikers");
        }
    }
}
