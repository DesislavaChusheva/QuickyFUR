using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickyFUR.Infrastructure.Migrations
{
    public partial class ConfiguratedProductSoldPropAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Sold",
                table: "ConfiguratedProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sold",
                table: "ConfiguratedProducts");
        }
    }
}
