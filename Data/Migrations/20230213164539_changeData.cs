using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class changeData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ReceiptDetails",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DiscountUnitPrice", "UnitPrice" },
                values: new object[] { 90m, 98m });

            migrationBuilder.UpdateData(
                table: "ReceiptDetails",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DiscountUnitPrice", "UnitPrice" },
                values: new object[] { 85m, 90m });

            migrationBuilder.UpdateData(
                table: "ReceiptDetails",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DiscountUnitPrice", "UnitPrice" },
                values: new object[] { 60m, 60m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ReceiptDetails",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DiscountUnitPrice", "UnitPrice" },
                values: new object[] { 10m, 196m });

            migrationBuilder.UpdateData(
                table: "ReceiptDetails",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DiscountUnitPrice", "UnitPrice" },
                values: new object[] { 30m, 630m });

            migrationBuilder.UpdateData(
                table: "ReceiptDetails",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DiscountUnitPrice", "UnitPrice" },
                values: new object[] { 18m, 120m });
        }
    }
}
