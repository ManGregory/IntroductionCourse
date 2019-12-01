using Microsoft.EntityFrameworkCore.Migrations;

namespace WebLMS.Data.Migrations
{
    public partial class Addnumbertolecture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Lectures",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Lectures");
        }
    }
}
