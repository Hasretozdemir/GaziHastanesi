using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GaziHastane.Migrations
{
    /// <inheritdoc />
    public partial class AddEgitimIcerikleriTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EgitimKomitesi");

            migrationBuilder.CreateTable(
                name: "EgitimIcerikleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Baslik = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    KisaAciklama = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Ikon = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Renk = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Hedef = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Icerik = table.Column<string>(type: "text", nullable: true),
                    FotoUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DosyaUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EgitimIcerikleri", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EgitimIcerikleri");

            migrationBuilder.CreateTable(
                name: "EgitimKomitesi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FotografUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Gorev = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Unvan = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    UyeAdSoyad = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EgitimKomitesi", x => x.Id);
                });
        }
    }
}
