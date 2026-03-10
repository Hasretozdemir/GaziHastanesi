using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GaziHastane.Migrations
{
    /// <inheritdoc />
    public partial class KategoriSutunuEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bolumler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Aciklama = table.Column<string>(type: "text", nullable: true),
                    FotografUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Kategori = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bolumler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Duyurular",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Baslik = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Icerik = table.Column<string>(type: "text", nullable: false),
                    GorselUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    YayinTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Duyurular", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EgitimKomitesi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UyeAdSoyad = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Gorev = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    FotografUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EgitimKomitesi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KaliteBelgeleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BelgeAdi = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Kategori = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DosyaUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    YayinTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KaliteBelgeleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TCKimlikNo = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    Ad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Soyad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DogumTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Telefon = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    SifreHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Cinsiyet = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    KullaniciTipi = table.Column<short>(type: "smallint", nullable: false),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YemekListesi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tarih = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Ogun = table.Column<short>(type: "smallint", nullable: false),
                    Corba = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    AnaYemek = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    YardimciYemek = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    TatliMeyve = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    ToplamKalori = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YemekListesi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BorclarOdemeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HastaId = table.Column<int>(type: "integer", nullable: true),
                    IslemTipi = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Tutar = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    OdendiMi = table.Column<bool>(type: "boolean", nullable: false),
                    SonOdemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OdemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DekontNo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorclarOdemeler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorclarOdemeler_Users_HastaId",
                        column: x => x.HastaId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Doktorlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KullaniciId = table.Column<int>(type: "integer", nullable: true),
                    BolumId = table.Column<int>(type: "integer", nullable: true),
                    Unvan = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Ad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Soyad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UzmanlikAlani = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Ozgecmis = table.Column<string>(type: "text", nullable: true),
                    FotografUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doktorlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doktorlar_Bolumler_BolumId",
                        column: x => x.BolumId,
                        principalTable: "Bolumler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Doktorlar_Users_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Randevular",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HastaId = table.Column<int>(type: "integer", nullable: true),
                    DoktorId = table.Column<int>(type: "integer", nullable: true),
                    BolumId = table.Column<int>(type: "integer", nullable: true),
                    RandevuTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Durum = table.Column<short>(type: "smallint", nullable: false),
                    Sikayet = table.Column<string>(type: "text", nullable: true),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Randevular", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Randevular_Bolumler_BolumId",
                        column: x => x.BolumId,
                        principalTable: "Bolumler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Randevular_Doktorlar_DoktorId",
                        column: x => x.DoktorId,
                        principalTable: "Doktorlar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Randevular_Users_HastaId",
                        column: x => x.HastaId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TahlilSonuclari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HastaId = table.Column<int>(type: "integer", nullable: true),
                    DoktorId = table.Column<int>(type: "integer", nullable: true),
                    Tarih = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TestKategorisi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TestAdi = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    SonucDegeri = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ReferansAraligi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    RaporDosyaUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TahlilSonuclari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TahlilSonuclari_Doktorlar_DoktorId",
                        column: x => x.DoktorId,
                        principalTable: "Doktorlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TahlilSonuclari_Users_HastaId",
                        column: x => x.HastaId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BorclarOdemeler_HastaId",
                table: "BorclarOdemeler",
                column: "HastaId");

            migrationBuilder.CreateIndex(
                name: "IX_Doktorlar_BolumId",
                table: "Doktorlar",
                column: "BolumId");

            migrationBuilder.CreateIndex(
                name: "IX_Doktorlar_KullaniciId",
                table: "Doktorlar",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_BolumId",
                table: "Randevular",
                column: "BolumId");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_DoktorId",
                table: "Randevular",
                column: "DoktorId");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_HastaId",
                table: "Randevular",
                column: "HastaId");

            migrationBuilder.CreateIndex(
                name: "IX_TahlilSonuclari_DoktorId",
                table: "TahlilSonuclari",
                column: "DoktorId");

            migrationBuilder.CreateIndex(
                name: "IX_TahlilSonuclari_HastaId",
                table: "TahlilSonuclari",
                column: "HastaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BorclarOdemeler");

            migrationBuilder.DropTable(
                name: "Duyurular");

            migrationBuilder.DropTable(
                name: "EgitimKomitesi");

            migrationBuilder.DropTable(
                name: "KaliteBelgeleri");

            migrationBuilder.DropTable(
                name: "Randevular");

            migrationBuilder.DropTable(
                name: "TahlilSonuclari");

            migrationBuilder.DropTable(
                name: "YemekListesi");

            migrationBuilder.DropTable(
                name: "Doktorlar");

            migrationBuilder.DropTable(
                name: "Bolumler");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
