using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GaziHastane.Migrations
{
    /// <inheritdoc />
    public partial class AddBilgiIslemSekmeleri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IconClass",
                table: "HemsirelikSekmeler",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "MedyaYolu",
                table: "HemsirelikIcerikler",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Baslik",
                table: "HemsirelikIcerikler",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AltBaslik",
                table: "HemsirelikIcerikler",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Aciklama",
                table: "HemsirelikIcerikler",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "oda_konumu",
                table: "Doktorlar",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BasinKurumsalIletisim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Baslik = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Aciklama = table.Column<string>(type: "text", nullable: false),
                    Telefon = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Lokasyon = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    SonGuncelleme = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasinKurumsalIletisim", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BilgiIslemSekmeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Baslik = table.Column<string>(type: "text", nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BilgiIslemSekmeler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KurumsalIcerikler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SayfaKey = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Kategori = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IcerikTipi = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Baslik = table.Column<string>(type: "text", nullable: true),
                    AltBaslik = table.Column<string>(type: "text", nullable: true),
                    Aciklama = table.Column<string>(type: "text", nullable: true),
                    MedyaYolu = table.Column<string>(type: "text", nullable: true),
                    VideoUrl = table.Column<string>(type: "text", nullable: true),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KurumsalIcerikler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KurumsalSekmeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Baslik = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SekmeId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Icerik = table.Column<string>(type: "text", nullable: true),
                    IconClass = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    SayfaKey = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KurumsalSekmeler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BilgiIslemIcerikler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SekmeId = table.Column<int>(type: "integer", nullable: false),
                    Tipi = table.Column<int>(type: "integer", nullable: false),
                    Baslik = table.Column<string>(type: "text", nullable: true),
                    MetinIcerik = table.Column<string>(type: "text", nullable: true),
                    DosyaYolu = table.Column<string>(type: "text", nullable: true),
                    VideoUrl = table.Column<string>(type: "text", nullable: true),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BilgiIslemIcerikler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BilgiIslemIcerikler_BilgiIslemSekmeler_SekmeId",
                        column: x => x.SekmeId,
                        principalTable: "BilgiIslemSekmeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BilgiIslemIcerikler_SekmeId",
                table: "BilgiIslemIcerikler",
                column: "SekmeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasinKurumsalIletisim");

            migrationBuilder.DropTable(
                name: "BilgiIslemIcerikler");

            migrationBuilder.DropTable(
                name: "KurumsalIcerikler");

            migrationBuilder.DropTable(
                name: "KurumsalSekmeler");

            migrationBuilder.DropTable(
                name: "BilgiIslemSekmeler");

            migrationBuilder.DropColumn(
                name: "oda_konumu",
                table: "Doktorlar");

            migrationBuilder.AlterColumn<string>(
                name: "IconClass",
                table: "HemsirelikSekmeler",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MedyaYolu",
                table: "HemsirelikIcerikler",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Baslik",
                table: "HemsirelikIcerikler",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AltBaslik",
                table: "HemsirelikIcerikler",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Aciklama",
                table: "HemsirelikIcerikler",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
