using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GaziHastane.Migrations
{
    /// <inheritdoc />
    public partial class IletisimVeUlasimEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "telefon",
                table: "Iletisim");

            migrationBuilder.RenameColumn(
                name: "isactive",
                table: "Iletisim",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "haritaurl",
                table: "Iletisim",
                newName: "HaritaUrl");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Iletisim",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "calismasaatleri",
                table: "Iletisim",
                newName: "CalismaSaatleri");

            migrationBuilder.RenameColumn(
                name: "baslik",
                table: "Iletisim",
                newName: "Baslik");

            migrationBuilder.RenameColumn(
                name: "adres",
                table: "Iletisim",
                newName: "Adres");

            migrationBuilder.AlterColumn<string>(
                name: "CalismaSaatleri",
                table: "Iletisim",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AltBaslik",
                table: "Iletisim",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CagriMerkezi",
                table: "Iletisim",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DigerTelefonlar",
                table: "Iletisim",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EkBilgi",
                table: "Iletisim",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KisaAdres",
                table: "Iletisim",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Koordinat",
                table: "Iletisim",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Santral",
                table: "Iletisim",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TemaRengi",
                table: "Iletisim",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UlasimRehberi");

            migrationBuilder.DropColumn(
                name: "AltBaslik",
                table: "Iletisim");

            migrationBuilder.DropColumn(
                name: "CagriMerkezi",
                table: "Iletisim");

            migrationBuilder.DropColumn(
                name: "DigerTelefonlar",
                table: "Iletisim");

            migrationBuilder.DropColumn(
                name: "EkBilgi",
                table: "Iletisim");

            migrationBuilder.DropColumn(
                name: "KisaAdres",
                table: "Iletisim");

            migrationBuilder.DropColumn(
                name: "Koordinat",
                table: "Iletisim");

            migrationBuilder.DropColumn(
                name: "Santral",
                table: "Iletisim");

            migrationBuilder.DropColumn(
                name: "TemaRengi",
                table: "Iletisim");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Iletisim",
                newName: "isactive");

            migrationBuilder.RenameColumn(
                name: "HaritaUrl",
                table: "Iletisim",
                newName: "haritaurl");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Iletisim",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "CalismaSaatleri",
                table: "Iletisim",
                newName: "calismasaatleri");

            migrationBuilder.RenameColumn(
                name: "Baslik",
                table: "Iletisim",
                newName: "baslik");

            migrationBuilder.RenameColumn(
                name: "Adres",
                table: "Iletisim",
                newName: "adres");

            migrationBuilder.AlterColumn<string>(
                name: "calismasaatleri",
                table: "Iletisim",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "telefon",
                table: "Iletisim",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
