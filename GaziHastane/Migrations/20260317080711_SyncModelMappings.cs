using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GaziHastane.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Use conditional SQL to avoid failures when lowercase tables/constraints are not present
            migrationBuilder.Sql(@"
ALTER TABLE IF EXISTS doktorlar DROP CONSTRAINT IF EXISTS ""FK_doktorlar_Users_kullaniciid"";
ALTER TABLE IF EXISTS doktorlar DROP CONSTRAINT IF EXISTS ""FK_doktorlar_bolumler_bolumid"";
ALTER TABLE IF EXISTS ""Randevular"" DROP CONSTRAINT IF EXISTS ""FK_Randevular_bolumler_BolumId"";
ALTER TABLE IF EXISTS ""Randevular"" DROP CONSTRAINT IF EXISTS ""FK_Randevular_doktorlar_DoktorId"";
ALTER TABLE IF EXISTS ""TahlilSonuclari"" DROP CONSTRAINT IF EXISTS ""FK_TahlilSonuclari_doktorlar_DoktorId"";

ALTER TABLE IF EXISTS doktorlar DROP CONSTRAINT IF EXISTS ""PK_doktorlar"";
ALTER TABLE IF EXISTS bolumler DROP CONSTRAINT IF EXISTS ""PK_bolumler"";

DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_class c JOIN pg_namespace n ON n.oid=c.relnamespace WHERE c.relname='doktorlar') THEN
        EXECUTE 'ALTER TABLE doktorlar RENAME TO ""Doktorlar""';
    END IF;
    IF EXISTS (SELECT 1 FROM pg_class c JOIN pg_namespace n ON n.oid=c.relnamespace WHERE c.relname='bolumler') THEN
        EXECUTE 'ALTER TABLE bolumler RENAME TO ""Bolumler""';
    END IF;
END
$$;

ALTER INDEX IF EXISTS IX_doktorlar_kullaniciid RENAME TO ""IX_Doktorlar_kullaniciid"";
ALTER INDEX IF EXISTS IX_doktorlar_bolumid RENAME TO ""IX_Doktorlar_bolumid"";
");

            migrationBuilder.RenameColumn(
                name: "sirano",
                table: "HastaRehberi",
                newName: "SiraNo");

            migrationBuilder.RenameColumn(
                name: "isactive",
                table: "HastaRehberi",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "ikon",
                table: "HastaRehberi",
                newName: "Ikon");

            migrationBuilder.RenameColumn(
                name: "icerik",
                table: "HastaRehberi",
                newName: "Icerik");

            migrationBuilder.RenameColumn(
                name: "baslik",
                table: "HastaRehberi",
                newName: "Baslik");

            migrationBuilder.AddColumn<string>(
                name: "Tema",
                table: "HastaRehberi",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Doktorlar",
                table: "Doktorlar",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bolumler",
                table: "Bolumler",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Doktorlar_Bolumler_bolumid",
                table: "Doktorlar",
                column: "bolumid",
                principalTable: "Bolumler",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Doktorlar_Users_kullaniciid",
                table: "Doktorlar",
                column: "kullaniciid",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Bolumler_BolumId",
                table: "Randevular",
                column: "BolumId",
                principalTable: "Bolumler",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Doktorlar_DoktorId",
                table: "Randevular",
                column: "DoktorId",
                principalTable: "Doktorlar",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_TahlilSonuclari_Doktorlar_DoktorId",
                table: "TahlilSonuclari",
                column: "DoktorId",
                principalTable: "Doktorlar",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doktorlar_Bolumler_bolumid",
                table: "Doktorlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Doktorlar_Users_kullaniciid",
                table: "Doktorlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Bolumler_BolumId",
                table: "Randevular");

            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Doktorlar_DoktorId",
                table: "Randevular");

            migrationBuilder.DropForeignKey(
                name: "FK_TahlilSonuclari_Doktorlar_DoktorId",
                table: "TahlilSonuclari");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Doktorlar",
                table: "Doktorlar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bolumler",
                table: "Bolumler");

            migrationBuilder.DropColumn(
                name: "Tema",
                table: "HastaRehberi");

            migrationBuilder.RenameTable(
                name: "Doktorlar",
                newName: "doktorlar");

            migrationBuilder.RenameTable(
                name: "Bolumler",
                newName: "bolumler");

            migrationBuilder.RenameColumn(
                name: "SiraNo",
                table: "HastaRehberi",
                newName: "sirano");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "HastaRehberi",
                newName: "isactive");

            migrationBuilder.RenameColumn(
                name: "Ikon",
                table: "HastaRehberi",
                newName: "ikon");

            migrationBuilder.RenameColumn(
                name: "Icerik",
                table: "HastaRehberi",
                newName: "icerik");

            migrationBuilder.RenameColumn(
                name: "Baslik",
                table: "HastaRehberi",
                newName: "baslik");

            migrationBuilder.RenameIndex(
                name: "IX_Doktorlar_kullaniciid",
                table: "doktorlar",
                newName: "IX_doktorlar_kullaniciid");

            migrationBuilder.RenameIndex(
                name: "IX_Doktorlar_bolumid",
                table: "doktorlar",
                newName: "IX_doktorlar_bolumid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_doktorlar",
                table: "doktorlar",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bolumler",
                table: "bolumler",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_doktorlar_Users_kullaniciid",
                table: "doktorlar",
                column: "kullaniciid",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_doktorlar_bolumler_bolumid",
                table: "doktorlar",
                column: "bolumid",
                principalTable: "bolumler",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_bolumler_BolumId",
                table: "Randevular",
                column: "BolumId",
                principalTable: "bolumler",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_doktorlar_DoktorId",
                table: "Randevular",
                column: "DoktorId",
                principalTable: "doktorlar",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_TahlilSonuclari_doktorlar_DoktorId",
                table: "TahlilSonuclari",
                column: "DoktorId",
                principalTable: "doktorlar",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
