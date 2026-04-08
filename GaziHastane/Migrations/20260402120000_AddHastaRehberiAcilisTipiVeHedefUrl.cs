using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GaziHastane.Migrations
{
    public partial class AddHastaRehberiAcilisTipiVeHedefUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcilisTipi",
                table: "HastaRehberi",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Modal");

            migrationBuilder.AddColumn<string>(
                name: "HedefUrl",
                table: "HastaRehberi",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcilisTipi",
                table: "HastaRehberi");

            migrationBuilder.DropColumn(
                name: "HedefUrl",
                table: "HastaRehberi");
        }
    }
}
