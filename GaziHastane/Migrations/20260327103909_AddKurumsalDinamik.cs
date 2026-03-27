using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GaziHastane.Migrations
{
    /// <inheritdoc />
    public partial class AddKurumsalDinamik : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Baslik",
                table: "KurumsalSayfalar",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AktifMi",
                table: "KurumsalSayfalar",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BashekimAdSoyad",
                table: "HastanemizIcerikler",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BashekimFotoUrl",
                table: "HastanemizIcerikler",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BashekimMesaj",
                table: "HastanemizIcerikler",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BasmudurAdSoyad",
                table: "HastanemizIcerikler",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BasmudurFotoUrl",
                table: "HastanemizIcerikler",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BasmudurMesaj",
                table: "HastanemizIcerikler",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YonetimAciklama",
                table: "HastanemizIcerikler",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "KurumsalMenuGruplar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GrupAdi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KurumsalMenuGruplar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KurumsalMenuler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GrupId = table.Column<int>(type: "integer", nullable: false),
                    Baslik = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IconClass = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KurumsalMenuler");

            migrationBuilder.DropTable(
                name: "KurumsalMenuGruplar");

            migrationBuilder.DropColumn(
                name: "AktifMi",
                table: "KurumsalSayfalar");

            migrationBuilder.DropColumn(
                name: "BashekimAdSoyad",
                table: "HastanemizIcerikler");

            migrationBuilder.DropColumn(
                name: "BashekimFotoUrl",
                table: "HastanemizIcerikler");

            migrationBuilder.DropColumn(
                name: "BashekimMesaj",
                table: "HastanemizIcerikler");

            migrationBuilder.DropColumn(
                name: "BasmudurAdSoyad",
                table: "HastanemizIcerikler");

            migrationBuilder.DropColumn(
                name: "BasmudurFotoUrl",
                table: "HastanemizIcerikler");

            migrationBuilder.DropColumn(
                name: "BasmudurMesaj",
                table: "HastanemizIcerikler");

            migrationBuilder.DropColumn(
                name: "YonetimAciklama",
                table: "HastanemizIcerikler");

            migrationBuilder.AlterColumn<string>(
                name: "Baslik",
                table: "KurumsalSayfalar",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250);
        }
    }
}
