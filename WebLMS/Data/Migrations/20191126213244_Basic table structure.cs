using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebLMS.Data.Migrations
{
    public partial class Basictablestructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "Lectures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    AvailableFrom = table.Column<DateTime>(nullable: true),
                    AvailableTo = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lectures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodingHomeworks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LectureId = table.Column<int>(nullable: false),
                    Subject = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    TemplateCode = table.Column<string>(nullable: true),
                    CodingTestType = table.Column<int>(nullable: false),
                    EntryType = table.Column<string>(nullable: true),
                    EntryMethodName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodingHomeworks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodingHomeworks_Lectures_LectureId",
                        column: x => x.LectureId,
                        principalTable: "Lectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodingTests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodingHomeworkId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    InputParameters = table.Column<string>(nullable: true),
                    ExpectedResult = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodingTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodingTests_CodingHomeworks_CodingHomeworkId",
                        column: x => x.CodingHomeworkId,
                        principalTable: "CodingHomeworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodingHomeworkTestRuns",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodingTestId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    SourceCode = table.Column<string>(nullable: true),
                    Result = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodingHomeworkTestRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodingHomeworkTestRuns_CodingTests_CodingTestId",
                        column: x => x.CodingTestId,
                        principalTable: "CodingTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CodingHomeworkTestRuns_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodingHomeworks_LectureId",
                table: "CodingHomeworks",
                column: "LectureId");

            migrationBuilder.CreateIndex(
                name: "IX_CodingHomeworkTestRuns_CodingTestId",
                table: "CodingHomeworkTestRuns",
                column: "CodingTestId");

            migrationBuilder.CreateIndex(
                name: "IX_CodingHomeworkTestRuns_UserId",
                table: "CodingHomeworkTestRuns",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CodingTests_CodingHomeworkId",
                table: "CodingTests",
                column: "CodingHomeworkId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodingHomeworkTestRuns");

            migrationBuilder.DropTable(
                name: "CodingTests");

            migrationBuilder.DropTable(
                name: "CodingHomeworks");

            migrationBuilder.DropTable(
                name: "Lectures");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
