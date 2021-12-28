using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qoqo.Migrations
{
    public partial class MoveOfferToOrderAndRemoveClick : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Clicks_ClickId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ClickId",
                table: "Orders",
                newName: "OfferId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ClickId",
                table: "Orders",
                newName: "IX_Orders_OfferId");

            migrationBuilder.AddColumn<int>(
                name: "OfferId1",
                table: "Offers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offers_OfferId1",
                table: "Offers",
                column: "OfferId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Offers_OfferId1",
                table: "Offers",
                column: "OfferId1",
                principalTable: "Offers",
                principalColumn: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Offers_OfferId",
                table: "Orders",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "OfferId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Offers_OfferId1",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Offers_OfferId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Offers_OfferId1",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "OfferId1",
                table: "Offers");

            migrationBuilder.RenameColumn(
                name: "OfferId",
                table: "Orders",
                newName: "ClickId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_OfferId",
                table: "Orders",
                newName: "IX_Orders_ClickId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Clicks_ClickId",
                table: "Orders",
                column: "ClickId",
                principalTable: "Clicks",
                principalColumn: "ClickId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
