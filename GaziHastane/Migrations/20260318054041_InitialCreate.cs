using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GaziHastane.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bolumler",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    aciklama = table.Column<string>(type: "text", nullable: true),
                    fotografurl = table.Column<string>(type: "text", nullable: true),
                    kategori = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bolumler", x => x.id);
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
                    YayinTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
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
                    Unvan = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    FotografUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EgitimKomitesi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HastaRehberi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Baslik = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Icerik = table.Column<string>(type: "text", nullable: false),
                    Ikon = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SiraNo = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Tema = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HastaRehberi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Iletisim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Baslik = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    AltBaslik = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    KisaAdres = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Koordinat = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Adres = table.Column<string>(type: "text", nullable: false),
                    CagriMerkezi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Santral = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DigerTelefonlar = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    CalismaSaatleri = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    EkBilgi = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    HaritaUrl = table.Column<string>(type: "text", nullable: true),
                    TemaRengi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Iletisim", x => x.Id);
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
                    YayinTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KaliteBelgeleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UlasimRehberi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UlasimTipi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Ikon = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Icerik = table.Column<string>(type: "text", nullable: false),
                    TemaRengi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UlasimRehberi", x => x.Id);
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
                    DogumTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Telefon = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    SifreHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Cinsiyet = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    KullaniciTipi = table.Column<short>(type: "smallint", nullable: false),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
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
                    Tarih = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
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
                    SonOdemeTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    OdemeTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kullaniciid = table.Column<int>(type: "integer", nullable: true),
                    bolumid = table.Column<int>(type: "integer", nullable: true),
                    unvan = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    soyad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    uzmanlikalani = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    ozgecmis = table.Column<string>(type: "text", nullable: true),
                    fotografurl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doktorlar", x => x.id);
                    table.ForeignKey(
                        name: "FK_Doktorlar_Bolumler_bolumid",
                        column: x => x.bolumid,
                        principalTable: "Bolumler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Doktorlar_Users_kullaniciid",
                        column: x => x.kullaniciid,
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
                    RandevuTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Durum = table.Column<short>(type: "smallint", nullable: false),
                    Sikayet = table.Column<string>(type: "text", nullable: true),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Randevular", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Randevular_Bolumler_BolumId",
                        column: x => x.BolumId,
                        principalTable: "Bolumler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Randevular_Doktorlar_DoktorId",
                        column: x => x.DoktorId,
                        principalTable: "Doktorlar",
                        principalColumn: "id");
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
                    Tarih = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
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
                        principalColumn: "id",
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
                name: "IX_Doktorlar_bolumid",
                table: "Doktorlar",
                column: "bolumid");

            migrationBuilder.CreateIndex(
                name: "IX_Doktorlar_kullaniciid",
                table: "Doktorlar",
                column: "kullaniciid");

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
                name: "HastaRehberi");

            migrationBuilder.DropTable(
                name: "Iletisim");

            migrationBuilder.DropTable(
                name: "KaliteBelgeleri");

            migrationBuilder.DropTable(
                name: "Randevular");

            migrationBuilder.DropTable(
                name: "TahlilSonuclari");

            migrationBuilder.DropTable(
                name: "UlasimRehberi");

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
