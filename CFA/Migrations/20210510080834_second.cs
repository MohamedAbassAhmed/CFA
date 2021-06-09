using Microsoft.EntityFrameworkCore.Migrations;

namespace CFA.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_PurchaseOrderStatuses_PurchaseOrderStatusNo",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_PurchaseOrderStatusNo",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderStatusNo",
                table: "PurchaseOrders");

            migrationBuilder.AddColumn<string>(
                name: "PurchaseOrderStatus",
                table: "PurchaseOrders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchaseOrderStatus",
                table: "PurchaseOrders");

            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderStatusNo",
                table: "PurchaseOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PurchaseOrderStatusNo",
                table: "PurchaseOrders",
                column: "PurchaseOrderStatusNo");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_PurchaseOrderStatuses_PurchaseOrderStatusNo",
                table: "PurchaseOrders",
                column: "PurchaseOrderStatusNo",
                principalTable: "PurchaseOrderStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
