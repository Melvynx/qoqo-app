using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qoqo.Migrations
{
    public partial class AddWinnerTextToOffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WinnerText",
                table: "Offers",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WinnerText",
                table: "Offers");
        }
    }
}
