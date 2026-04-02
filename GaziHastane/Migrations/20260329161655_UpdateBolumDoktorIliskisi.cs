using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GaziHastane.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBolumDoktorIliskisi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Bolumler_BolumId",
                table: "Randevular");

            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Doktorlar_DoktorId",
                table: "Randevular");

            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Users_HastaId",
                table: "Randevular");

            migrationBuilder.RenameColumn(
                name: "Sikayet",
                table: "Randevular",
                newName: "sikayet");

            migrationBuilder.RenameColumn(
                name: "RandevuTarihi",
                table: "Randevular",
                newName: "randevutarihi");

            migrationBuilder.RenameColumn(
                name: "OlusturulmaTarihi",
                table: "Randevular",
                newName: "olusturulmatarihi");

            migrationBuilder.RenameColumn(
                name: "HastaId",
                table: "Randevular",
                newName: "hastaid");

            migrationBuilder.RenameColumn(
                name: "Durum",
                table: "Randevular",
                newName: "durum");

            migrationBuilder.RenameColumn(
                name: "DoktorId",
                table: "Randevular",
                newName: "doktorid");

            migrationBuilder.RenameColumn(
                name: "BolumId",
                table: "Randevular",
                newName: "bolumid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Randevular",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Randevular_HastaId",
                table: "Randevular",
                newName: "IX_Randevular_hastaid");

            migrationBuilder.RenameIndex(
                name: "IX_Randevular_DoktorId",
                table: "Randevular",
                newName: "IX_Randevular_doktorid");

            migrationBuilder.RenameIndex(
                name: "IX_Randevular_BolumId",
                table: "Randevular",
                newName: "IX_Randevular_bolumid");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Bolumler_bolumid",
                table: "Randevular",
                column: "bolumid",
                principalTable: "Bolumler",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Doktorlar_doktorid",
                table: "Randevular",
                column: "doktorid",
                principalTable: "Doktorlar",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Users_hastaid",
                table: "Randevular",
                column: "hastaid",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Bolumler_bolumid",
                table: "Randevular");

            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Doktorlar_doktorid",
                table: "Randevular");

            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Users_hastaid",
                table: "Randevular");

            migrationBuilder.RenameColumn(
                name: "sikayet",
                table: "Randevular",
                newName: "Sikayet");

            migrationBuilder.RenameColumn(
                name: "randevutarihi",
                table: "Randevular",
                newName: "RandevuTarihi");

            migrationBuilder.RenameColumn(
                name: "olusturulmatarihi",
                table: "Randevular",
                newName: "OlusturulmaTarihi");

            migrationBuilder.RenameColumn(
                name: "hastaid",
                table: "Randevular",
                newName: "HastaId");

            migrationBuilder.RenameColumn(
                name: "durum",
                table: "Randevular",
                newName: "Durum");

            migrationBuilder.RenameColumn(
                name: "doktorid",
                table: "Randevular",
                newName: "DoktorId");

            migrationBuilder.RenameColumn(
                name: "bolumid",
                table: "Randevular",
                newName: "BolumId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Randevular",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Randevular_hastaid",
                table: "Randevular",
                newName: "IX_Randevular_HastaId");

            migrationBuilder.RenameIndex(
                name: "IX_Randevular_doktorid",
                table: "Randevular",
                newName: "IX_Randevular_DoktorId");

            migrationBuilder.RenameIndex(
                name: "IX_Randevular_bolumid",
                table: "Randevular",
                newName: "IX_Randevular_BolumId");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Bolumler_BolumId",
                table: "Randevular",
                column: "BolumId",
                principalTable: "Bolumler",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Doktorlar_DoktorId",
                table: "Randevular",
                column: "DoktorId",
                principalTable: "Doktorlar",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Users_HastaId",
                table: "Randevular",
                column: "HastaId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
