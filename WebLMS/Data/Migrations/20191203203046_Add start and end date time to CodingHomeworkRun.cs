using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebLMS.Data.Migrations
{
    public partial class AddstartandenddatetimetoCodingHomeworkRun : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodingHomeworkRun_CodingHomeworks_CodingHomeworkId",
                table: "CodingHomeworkRun");

            migrationBuilder.DropForeignKey(
                name: "FK_CodingHomeworkRun_AspNetUsers_UserId",
                table: "CodingHomeworkRun");

            migrationBuilder.DropForeignKey(
                name: "FK_CodingHomeworkTestRuns_CodingHomeworkRun_CodingHomeworkRunId",
                table: "CodingHomeworkTestRuns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CodingHomeworkRun",
                table: "CodingHomeworkRun");

            migrationBuilder.RenameTable(
                name: "CodingHomeworkRun",
                newName: "CodingHomeworkRuns");

            migrationBuilder.RenameIndex(
                name: "IX_CodingHomeworkRun_UserId",
                table: "CodingHomeworkRuns",
                newName: "IX_CodingHomeworkRuns_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CodingHomeworkRun_CodingHomeworkId",
                table: "CodingHomeworkRuns",
                newName: "IX_CodingHomeworkRuns_CodingHomeworkId");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "CodingHomeworkRuns",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "CodingHomeworkRuns",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CodingHomeworkRuns",
                table: "CodingHomeworkRuns",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CodingHomeworkRuns_CodingHomeworks_CodingHomeworkId",
                table: "CodingHomeworkRuns",
                column: "CodingHomeworkId",
                principalTable: "CodingHomeworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CodingHomeworkRuns_AspNetUsers_UserId",
                table: "CodingHomeworkRuns",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CodingHomeworkTestRuns_CodingHomeworkRuns_CodingHomeworkRunId",
                table: "CodingHomeworkTestRuns",
                column: "CodingHomeworkRunId",
                principalTable: "CodingHomeworkRuns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodingHomeworkRuns_CodingHomeworks_CodingHomeworkId",
                table: "CodingHomeworkRuns");

            migrationBuilder.DropForeignKey(
                name: "FK_CodingHomeworkRuns_AspNetUsers_UserId",
                table: "CodingHomeworkRuns");

            migrationBuilder.DropForeignKey(
                name: "FK_CodingHomeworkTestRuns_CodingHomeworkRuns_CodingHomeworkRunId",
                table: "CodingHomeworkTestRuns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CodingHomeworkRuns",
                table: "CodingHomeworkRuns");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "CodingHomeworkRuns");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "CodingHomeworkRuns");

            migrationBuilder.RenameTable(
                name: "CodingHomeworkRuns",
                newName: "CodingHomeworkRun");

            migrationBuilder.RenameIndex(
                name: "IX_CodingHomeworkRuns_UserId",
                table: "CodingHomeworkRun",
                newName: "IX_CodingHomeworkRun_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CodingHomeworkRuns_CodingHomeworkId",
                table: "CodingHomeworkRun",
                newName: "IX_CodingHomeworkRun_CodingHomeworkId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CodingHomeworkRun",
                table: "CodingHomeworkRun",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CodingHomeworkRun_CodingHomeworks_CodingHomeworkId",
                table: "CodingHomeworkRun",
                column: "CodingHomeworkId",
                principalTable: "CodingHomeworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CodingHomeworkRun_AspNetUsers_UserId",
                table: "CodingHomeworkRun",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CodingHomeworkTestRuns_CodingHomeworkRun_CodingHomeworkRunId",
                table: "CodingHomeworkTestRuns",
                column: "CodingHomeworkRunId",
                principalTable: "CodingHomeworkRun",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
