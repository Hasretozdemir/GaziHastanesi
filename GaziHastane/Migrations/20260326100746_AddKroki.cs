using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GaziHastane.Migrations
{
    /// <inheritdoc />
    public partial class AddKroki : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doktorlar_Bolumler_bolumid",
                table: "Doktorlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Bolumler_BolumId",
                table: "Randevular");

            migrationBuilder.DropForeignKey(
                name: "FK_TahlilSonuclari_Doktorlar_DoktorId",
                table: "TahlilSonuclari");

            migrationBuilder.AddColumn<string>(
                name: "blok",
                table: "Bolumler",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "kat",
                table: "Bolumler",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HastanemizIcerikler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HeroBaslik = table.Column<string>(type: "text", nullable: false),
                    HeroAltBaslik = table.Column<string>(type: "text", nullable: false),
                    Misyon = table.Column<string>(type: "text", nullable: false),
                    Vizyon = table.Column<string>(type: "text", nullable: false),
                    Tarihce = table.Column<string>(type: "text", nullable: false),
                    KlinikSayisi = table.Column<int>(type: "integer", nullable: false),
                    UzmanSayisi = table.Column<int>(type: "integer", nullable: false),
                    YatakKapasitesi = table.Column<int>(type: "integer", nullable: false),
                    KurulusYili = table.Column<int>(type: "integer", nullable: false),
                    Adres = table.Column<string>(type: "text", nullable: false),
                    Telefon = table.Column<string>(type: "text", nullable: false),
                    EPosta = table.Column<string>(type: "text", nullable: false),
                    GuncellemeZamani = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HastanemizIcerikler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KrokiBirimleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KatAdi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Baslik = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Aciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Ikon = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    GridColumn = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    GridRow = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TipSinifi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BolumId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KrokiBirimleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KrokiBirimleri_Bolumler_BolumId",
                        column: x => x.BolumId,
                        principalTable: "Bolumler",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_KrokiBirimleri_BolumId",
                table: "KrokiBirimleri",
                column: "BolumId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doktorlar_Bolumler_bolumid",
                table: "Doktorlar",
                column: "bolumid",
                principalTable: "Bolumler",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Bolumler_BolumId",
                table: "Randevular",
                column: "BolumId",
                principalTable: "Bolumler",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_TahlilSonuclari_Doktorlar_DoktorId",
                table: "TahlilSonuclari",
                column: "DoktorId",
                principalTable: "Doktorlar",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doktorlar_Bolumler_bolumid",
                table: "Doktorlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Bolumler_BolumId",
                table: "Randevular");

            migrationBuilder.DropForeignKey(
                name: "FK_TahlilSonuclari_Doktorlar_DoktorId",
                table: "TahlilSonuclari");

            migrationBuilder.DropTable(
                name: "HastanemizIcerikler");

            migrationBuilder.DropTable(
                name: "KrokiBirimleri");

            migrationBuilder.DropColumn(
                name: "blok",
                table: "Bolumler");

            migrationBuilder.DropColumn(
                name: "kat",
                table: "Bolumler");

            migrationBuilder.AddForeignKey(
                name: "FK_Doktorlar_Bolumler_bolumid",
                table: "Doktorlar",
                column: "bolumid",
                principalTable: "Bolumler",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Bolumler_BolumId",
                table: "Randevular",
                column: "BolumId",
                principalTable: "Bolumler",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TahlilSonuclari_Doktorlar_DoktorId",
                table: "TahlilSonuclari",
                column: "DoktorId",
                principalTable: "Doktorlar",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
