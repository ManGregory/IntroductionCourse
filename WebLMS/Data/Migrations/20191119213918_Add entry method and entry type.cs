using Microsoft.EntityFrameworkCore.Migrations;

namespace WebLMS.Data.Migrations
{
    public partial class Addentrymethodandentrytype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EntryMethodName",
                table: "CodingHomeworks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntryType",
                table: "CodingHomeworks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntryMethodName",
                table: "CodingHomeworks");

            migrationBuilder.DropColumn(
                name: "EntryType",
                table: "CodingHomeworks");
        }
    }
}
