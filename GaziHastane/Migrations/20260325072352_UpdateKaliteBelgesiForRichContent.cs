using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GaziHastane.Migrations
{
    /// <inheritdoc />
    public partial class UpdateKaliteBelgesiForRichContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Aciklama",
                table: "KaliteBelgeleri",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FotoUrl",
                table: "KaliteBelgeleri",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aciklama",
                table: "KaliteBelgeleri");

            migrationBuilder.DropColumn(
                name: "FotoUrl",
                table: "KaliteBelgeleri");
        }
    }
}
