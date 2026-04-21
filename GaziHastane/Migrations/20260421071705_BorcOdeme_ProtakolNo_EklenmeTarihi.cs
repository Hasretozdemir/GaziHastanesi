using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GaziHastane.Migrations
{
    /// <inheritdoc />
    public partial class BorcOdemeProtakolNoEklenmeTarihi : Migration
    {
        /// <inheritdoc />
                protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EklenmeTarihi",
                table: "BorclarOdemeler",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProtakolNo",
                table: "BorclarOdemeler",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminMenuItems");

            migrationBuilder.DropTable(
                name: "Belgeler");

            migrationBuilder.DropTable(
                name: "DoktorRandevuPlanGunleri");

            migrationBuilder.DropTable(
                name: "PanelAyarlar");

            migrationBuilder.DropTable(
                name: "DoktorRandevuPlanlari");

            migrationBuilder.DropColumn(
                name: "AdminSayfaYetkileri",
                table: "Yetkililer");

            migrationBuilder.DropColumn(
                name: "randevutipi",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "AcilisTipi",
                table: "HastaRehberi");

            migrationBuilder.DropColumn(
                name: "HedefUrl",
                table: "HastaRehberi");

            migrationBuilder.DropColumn(
                name: "ModalIcerik",
                table: "HastaRehberi");

            migrationBuilder.DropColumn(
                name: "EklenmeTarihi",
                table: "BorclarOdemeler");

            migrationBuilder.DropColumn(
                name: "ProtakolNo",
                table: "BorclarOdemeler");
        }
    }
}
