using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GaziHastane.Migrations
{
    public partial class AddAdminMenuItemsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminMenuItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Section = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PermissionKey = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Label = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    IconClass = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Controller = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Action = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ActiveIconClass = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    HoverIconClass = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsSuperAdminOnly = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminMenuItems", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminMenuItems");
        }
    }
}
