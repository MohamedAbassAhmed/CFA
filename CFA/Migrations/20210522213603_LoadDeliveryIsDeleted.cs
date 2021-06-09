using Microsoft.EntityFrameworkCore.Migrations;

namespace CFA.Migrations
{
    public partial class LoadDeliveryIsDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LoadDeliveries",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "LoadDeliveries");
        }
    }
}
