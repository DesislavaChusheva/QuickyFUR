using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickyFUR.Infrastructure.Migrations
{
    public partial class ConfProductRemovedPropAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Removed",
                table: "ConfiguratedProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Removed",
                table: "ConfiguratedProducts");
        }
    }
}
