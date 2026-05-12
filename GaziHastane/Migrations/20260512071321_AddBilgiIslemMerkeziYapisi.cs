using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GaziHastane.Migrations
{
    /// <inheritdoc />
    public partial class AddBilgiIslemMerkeziYapisi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BilgiIslemIcerikler");

            migrationBuilder.DropTable(
                name: "BilgiIslemSekmeler");

            migrationBuilder.CreateTable(
                name: "BilgiIslemMerkeziSekmeler",
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
                    table.PrimaryKey("PK_BilgiIslemMerkeziSekmeler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BilgiIslemMerkeziIcerikler",
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
                    table.PrimaryKey("PK_BilgiIslemMerkeziIcerikler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BilgiIslemMerkeziIcerikler_BilgiIslemMerkeziSekmeler_SekmeId",
                        column: x => x.SekmeId,
                        principalTable: "BilgiIslemMerkeziSekmeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BilgiIslemMerkeziIcerikler_SekmeId",
                table: "BilgiIslemMerkeziIcerikler",
                column: "SekmeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BilgiIslemMerkeziIcerikler");

            migrationBuilder.DropTable(
                name: "BilgiIslemMerkeziSekmeler");

            migrationBuilder.CreateTable(
                name: "BilgiIslemSekmeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Baslik = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BilgiIslemSekmeler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BilgiIslemIcerikler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SekmeId = table.Column<int>(type: "integer", nullable: false),
                    Baslik = table.Column<string>(type: "text", nullable: true),
                    DosyaYolu = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    MetinIcerik = table.Column<string>(type: "text", nullable: true),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    Tipi = table.Column<int>(type: "integer", nullable: false),
                    VideoUrl = table.Column<string>(type: "text", nullable: true)
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
    }
}
