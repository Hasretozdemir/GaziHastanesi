using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GaziHastane.Migrations
{
    /// <inheritdoc />
    public partial class AddKrokiBloklarTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HastanemizIcerikler");

            migrationBuilder.DropTable(
                name: "KurumsalMenuler");

            migrationBuilder.DropTable(
                name: "KurumsalSayfalar");

            migrationBuilder.DropTable(
                name: "KurumsalMenuGruplar");

            migrationBuilder.CreateTable(
                name: "KrokiBloklar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BlokAdi = table.Column<string>(type: "text", nullable: false),
                    Renk = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KrokiBloklar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KrokiKatlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KatAdi = table.Column<string>(type: "text", nullable: false),
                    BlokId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KrokiKatlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KrokiKatlar_KrokiBloklar_BlokId",
                        column: x => x.BlokId,
                        principalTable: "KrokiBloklar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KrokiBolumler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BirimAdi = table.Column<string>(type: "text", nullable: false),
                    KatId = table.Column<int>(type: "integer", nullable: false),
                    Ikon = table.Column<string>(type: "text", nullable: false),
                    IsEmpty = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KrokiBolumler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KrokiBolumler_KrokiKatlar_KatId",
                        column: x => x.KatId,
                        principalTable: "KrokiKatlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KrokiBolumler_KatId",
                table: "KrokiBolumler",
                column: "KatId");

            migrationBuilder.CreateIndex(
                name: "IX_KrokiKatlar_BlokId",
                table: "KrokiKatlar",
                column: "BlokId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KrokiBolumler");

            migrationBuilder.DropTable(
                name: "KrokiKatlar");

            migrationBuilder.DropTable(
                name: "KrokiBloklar");

            migrationBuilder.CreateTable(
                name: "HastanemizIcerikler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Adres = table.Column<string>(type: "text", nullable: false),
                    BashekimAdSoyad = table.Column<string>(type: "text", nullable: true),
                    BashekimFotoUrl = table.Column<string>(type: "text", nullable: true),
                    BashekimMesaj = table.Column<string>(type: "text", nullable: true),
                    BasmudurAdSoyad = table.Column<string>(type: "text", nullable: true),
                    BasmudurFotoUrl = table.Column<string>(type: "text", nullable: true),
                    BasmudurMesaj = table.Column<string>(type: "text", nullable: true),
                    EPosta = table.Column<string>(type: "text", nullable: false),
                    GuncellemeZamani = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    HeroAltBaslik = table.Column<string>(type: "text", nullable: false),
                    HeroBaslik = table.Column<string>(type: "text", nullable: false),
                    KlinikSayisi = table.Column<int>(type: "integer", nullable: false),
                    KurulusYili = table.Column<int>(type: "integer", nullable: false),
                    Misyon = table.Column<string>(type: "text", nullable: false),
                    Tarihce = table.Column<string>(type: "text", nullable: false),
                    Telefon = table.Column<string>(type: "text", nullable: false),
                    UzmanSayisi = table.Column<int>(type: "integer", nullable: false),
                    Vizyon = table.Column<string>(type: "text", nullable: false),
                    YatakKapasitesi = table.Column<int>(type: "integer", nullable: false),
                    YonetimAciklama = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HastanemizIcerikler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KurumsalMenuGruplar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    GrupAdi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KurumsalMenuGruplar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KurumsalSayfalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    Baslik = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    FotografUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    GuncellemeZamani = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Icerik = table.Column<string>(type: "text", nullable: true),
                    Slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Telefon = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Unvan = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KurumsalSayfalar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KurumsalMenuler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GrupId = table.Column<int>(type: "integer", nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    Baslik = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IconClass = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KurumsalMenuler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KurumsalMenuler_KurumsalMenuGruplar_GrupId",
                        column: x => x.GrupId,
                        principalTable: "KurumsalMenuGruplar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KurumsalMenuler_GrupId",
                table: "KurumsalMenuler",
                column: "GrupId");
        }
    }
}
