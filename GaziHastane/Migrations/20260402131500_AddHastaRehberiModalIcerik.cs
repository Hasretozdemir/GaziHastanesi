using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GaziHastane.Migrations
{
    public partial class AddHastaRehberiModalIcerik : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModalIcerik",
                table: "HastaRehberi",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModalIcerik",
                table: "HastaRehberi");
        }
    }
}
