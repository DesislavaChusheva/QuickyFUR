using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickyFUR.Infrastructure.Migrations
{
    public partial class ImageChangedFromFileToLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "ConfiguratedProducts");

            migrationBuilder.AddColumn<string>(
                name: "ImageLink",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageLink",
                table: "ConfiguratedProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageLink",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageLink",
                table: "ConfiguratedProducts");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Products",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "ConfiguratedProducts",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
