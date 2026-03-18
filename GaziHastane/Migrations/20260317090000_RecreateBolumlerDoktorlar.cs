using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GaziHastane.Migrations
{
    public partial class RecreateBolumlerDoktorlar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Ensure old tables removed if exist
            migrationBuilder.Sql("DROP TABLE IF EXISTS \"Doktorlar\" CASCADE;");
            migrationBuilder.Sql("DROP TABLE IF EXISTS \"Bolumler\" CASCADE;");

            migrationBuilder.CreateTable(
                name: "Bolumler",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    aciklama = table.Column<string>(type: "text", nullable: true),
                    fotografurl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    kategori = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bolumler", x => x.id);
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
                    isactive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doktorlar_bolumid",
                table: "Doktorlar",
                column: "bolumid");

            migrationBuilder.CreateIndex(
                name: "IX_Doktorlar_kullaniciid",
                table: "Doktorlar",
                column: "kullaniciid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Doktorlar");

            migrationBuilder.DropTable(
                name: "Bolumler");
        }
    }
}
