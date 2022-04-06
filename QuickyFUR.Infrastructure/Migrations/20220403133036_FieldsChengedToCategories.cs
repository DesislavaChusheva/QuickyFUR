using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickyFUR.Infrastructure.Migrations
{
    public partial class FieldsChengedToCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConfiguratedProducts_Fields_FieldId",
                table: "ConfiguratedProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Fields_FieldId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Fields");

            migrationBuilder.RenameColumn(
                name: "FieldId",
                table: "Products",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_FieldId",
                table: "Products",
                newName: "IX_Products_CategoryId");

            migrationBuilder.RenameColumn(
                name: "FieldId",
                table: "ConfiguratedProducts",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ConfiguratedProducts_FieldId",
                table: "ConfiguratedProducts",
                newName: "IX_ConfiguratedProducts_CategoryId");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ConfiguratedProducts_Categories_CategoryId",
                table: "ConfiguratedProducts",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConfiguratedProducts_Categories_CategoryId",
                table: "ConfiguratedProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Products",
                newName: "FieldId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                newName: "IX_Products_FieldId");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "ConfiguratedProducts",
                newName: "FieldId");

            migrationBuilder.RenameIndex(
                name: "IX_ConfiguratedProducts_CategoryId",
                table: "ConfiguratedProducts",
                newName: "IX_ConfiguratedProducts_FieldId");

            migrationBuilder.CreateTable(
                name: "Fields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fields", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ConfiguratedProducts_Fields_FieldId",
                table: "ConfiguratedProducts",
                column: "FieldId",
                principalTable: "Fields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Fields_FieldId",
                table: "Products",
                column: "FieldId",
                principalTable: "Fields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
