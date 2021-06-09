using Microsoft.EntityFrameworkCore.Migrations;

namespace CFA.Migrations
{
    public partial class addT_POSandT_LDS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoadDeliveryStocks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoadDeliveryNo = table.Column<int>(nullable: false),
                    Quantity = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadDeliveryStock", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_LoadDeliveryStocks_LoadDeliveries_LoadDeliveryNo",
                        column: x => x.LoadDeliveryNo,
                        principalTable: "LoadDeliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderSupplies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoadDeliveryNo = table.Column<int>(nullable: false),
                    PurchaseOrderNo = table.Column<int>(nullable: false),
                    Quantity = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderSupply", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderSupplies_LoadDeliveries_LoadDeliveryNo",
                        column: x => x.LoadDeliveryNo,
                        principalTable: "LoadDeliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderSupplies_PurchaseOrders_PurchaseOrderNo",
                        column: x => x.PurchaseOrderNo,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoadDeliveryStocks_LoadDeliveryNo",
                table: "LoadDeliveryStocks",
                column: "LoadDeliveryNo");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderSupplies_LoadDeliveryNo",
                table: "PurchaseOrderSupplies",
                column: "LoadDeliveryNo");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderSupplies_PurchaseOrderNo",
                table: "PurchaseOrderSupplies",
                column: "PurchaseOrderNo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoadDeliveryStocks");

            migrationBuilder.DropTable(
                name: "PurchaseOrderSupplies");
        }
    }
}
