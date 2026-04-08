using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GaziHastane.Migrations
{
    public partial class AddYetkiliAdminSayfaYetkileri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminSayfaYetkileri",
                table: "Yetkililer",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminSayfaYetkileri",
                table: "Yetkililer");
        }
    }
}
