using Microsoft.EntityFrameworkCore.Migrations;

namespace WebLMS.Data.Migrations
{
    public partial class fixtypo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxAttemtps",
                table: "CodingHomeworks");

            migrationBuilder.AddColumn<int>(
                name: "MaxAttempts",
                table: "CodingHomeworks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxAttempts",
                table: "CodingHomeworks");

            migrationBuilder.AddColumn<int>(
                name: "MaxAttemtps",
                table: "CodingHomeworks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
