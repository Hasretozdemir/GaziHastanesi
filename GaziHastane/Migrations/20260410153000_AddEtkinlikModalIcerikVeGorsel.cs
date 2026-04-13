using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using GaziHastane.Data;

#nullable disable

namespace GaziHastane.Migrations
{
    [DbContext(typeof(GaziHastaneContext))]
    [Migration("20260410153000_AddEtkinlikModalIcerikVeGorsel")]
    public partial class AddEtkinlikModalIcerikVeGorsel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "Etkinlikler"
                ADD COLUMN IF NOT EXISTS "GorselUrl" character varying(255);
                """);

            migrationBuilder.Sql("""
                ALTER TABLE "Etkinlikler"
                ADD COLUMN IF NOT EXISTS "ModalIcerik" text;
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "Etkinlikler"
                DROP COLUMN IF EXISTS "GorselUrl";
                """);

            migrationBuilder.Sql("""
                ALTER TABLE "Etkinlikler"
                DROP COLUMN IF EXISTS "ModalIcerik";
                """);
        }
    }
}
