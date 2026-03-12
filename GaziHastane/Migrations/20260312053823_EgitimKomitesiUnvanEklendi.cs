using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GaziHastane.Migrations
{
    /// <inheritdoc />
    public partial class EgitimKomitesiUnvanEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Sadece EgitimKomitesi tablosuna Unvan sütununu ekleyen kod
            migrationBuilder.AddColumn<string>(
                name: "Unvan",
                table: "EgitimKomitesi",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Migration geri alınırsa sadece Unvan sütununu silen kod
            migrationBuilder.DropColumn(
                name: "Unvan",
                table: "EgitimKomitesi");
        }
    }
}