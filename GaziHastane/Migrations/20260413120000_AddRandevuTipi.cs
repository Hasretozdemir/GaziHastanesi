using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using GaziHastane.Data;

#nullable disable

namespace GaziHastane.Migrations
{
    [DbContext(typeof(GaziHastaneContext))]
    [Migration("20260413120000_AddRandevuTipi")]
    public partial class AddRandevuTipi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "Randevular"
                ADD COLUMN IF NOT EXISTS "randevutipi" smallint NOT NULL DEFAULT 1;
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "Randevular"
                DROP COLUMN IF EXISTS "randevutipi";
                """);
        }
    }
}
