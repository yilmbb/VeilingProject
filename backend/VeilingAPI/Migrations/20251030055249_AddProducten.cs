using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeilingAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddProducten : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Producten",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    naam = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    beschrijving = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    prijs = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    voorraad = table.Column<int>(type: "int", nullable: false),
                    aangemaakt_op = table.Column<DateTime>(type: "datetime2", nullable: false),
                    verkoper_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producten", x => x.product_id);
                    table.ForeignKey(
                        name: "FK_Producten_Gebruikers_verkoper_id",
                        column: x => x.verkoper_id,
                        principalTable: "Gebruikers",
                        principalColumn: "gebruiker_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Producten_verkoper_id",
                table: "Producten",
                column: "verkoper_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Producten");
        }
    }
}
