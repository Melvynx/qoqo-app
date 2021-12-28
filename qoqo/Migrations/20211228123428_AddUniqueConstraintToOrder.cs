using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qoqo.Migrations
{
    public partial class AddUniqueConstraintToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_OfferId",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OfferId",
                table: "Orders",
                column: "OfferId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_OfferId",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OfferId",
                table: "Orders",
                column: "OfferId");
        }
    }
}
