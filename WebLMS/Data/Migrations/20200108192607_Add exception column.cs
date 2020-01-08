using Microsoft.EntityFrameworkCore.Migrations;

namespace WebLMS.Data.Migrations
{
    public partial class Addexceptioncolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Exception",
                table: "CodingHomeworkTestRuns",
                maxLength: 4000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Exception",
                table: "CodingHomeworkTestRuns");
        }
    }
}
