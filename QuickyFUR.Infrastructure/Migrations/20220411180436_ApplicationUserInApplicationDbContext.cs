using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickyFUR.Infrastructure.Migrations
{
    public partial class ApplicationUserInApplicationDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(747)",
                maxLength: 747,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(747)",
                oldMaxLength: 747,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(747)",
                maxLength: 747,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(747)",
                oldMaxLength: 747,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(747)",
                maxLength: 747,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(747)",
                oldMaxLength: 747);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(747)",
                maxLength: 747,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(747)",
                oldMaxLength: 747);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
