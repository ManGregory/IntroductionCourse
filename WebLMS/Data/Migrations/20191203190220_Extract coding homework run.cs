using Microsoft.EntityFrameworkCore.Migrations;

namespace WebLMS.Data.Migrations
{
    public partial class Extractcodinghomeworkrun : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodingHomeworkTestRuns_AspNetUsers_UserId",
                table: "CodingHomeworkTestRuns");

            migrationBuilder.DropIndex(
                name: "IX_CodingHomeworkTestRuns_UserId",
                table: "CodingHomeworkTestRuns");

            migrationBuilder.DropColumn(
                name: "SourceCode",
                table: "CodingHomeworkTestRuns");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CodingHomeworkTestRuns");

            migrationBuilder.AddColumn<int>(
                name: "CodingHomeworkRunId",
                table: "CodingHomeworkTestRuns",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompilation",
                table: "CodingHomeworkTestRuns",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "CodingHomeworkTestRuns",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TestRunStatus",
                table: "CodingHomeworkTestRuns",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CodingHomeworkRun",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    CodingHomeworkId = table.Column<int>(nullable: true),
                    SourceCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodingHomeworkRun", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodingHomeworkRun_CodingHomeworks_CodingHomeworkId",
                        column: x => x.CodingHomeworkId,
                        principalTable: "CodingHomeworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CodingHomeworkRun_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodingHomeworkTestRuns_CodingHomeworkRunId",
                table: "CodingHomeworkTestRuns",
                column: "CodingHomeworkRunId");

            migrationBuilder.CreateIndex(
                name: "IX_CodingHomeworkRun_CodingHomeworkId",
                table: "CodingHomeworkRun",
                column: "CodingHomeworkId");

            migrationBuilder.CreateIndex(
                name: "IX_CodingHomeworkRun_UserId",
                table: "CodingHomeworkRun",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CodingHomeworkTestRuns_CodingHomeworkRun_CodingHomeworkRunId",
                table: "CodingHomeworkTestRuns",
                column: "CodingHomeworkRunId",
                principalTable: "CodingHomeworkRun",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodingHomeworkTestRuns_CodingHomeworkRun_CodingHomeworkRunId",
                table: "CodingHomeworkTestRuns");

            migrationBuilder.DropTable(
                name: "CodingHomeworkRun");

            migrationBuilder.DropIndex(
                name: "IX_CodingHomeworkTestRuns_CodingHomeworkRunId",
                table: "CodingHomeworkTestRuns");

            migrationBuilder.DropColumn(
                name: "CodingHomeworkRunId",
                table: "CodingHomeworkTestRuns");

            migrationBuilder.DropColumn(
                name: "IsCompilation",
                table: "CodingHomeworkTestRuns");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "CodingHomeworkTestRuns");

            migrationBuilder.DropColumn(
                name: "TestRunStatus",
                table: "CodingHomeworkTestRuns");

            migrationBuilder.AddColumn<string>(
                name: "SourceCode",
                table: "CodingHomeworkTestRuns",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CodingHomeworkTestRuns",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CodingHomeworkTestRuns_UserId",
                table: "CodingHomeworkTestRuns",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CodingHomeworkTestRuns_AspNetUsers_UserId",
                table: "CodingHomeworkTestRuns",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
