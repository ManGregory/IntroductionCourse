﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebLMS.Data.Migrations
{
    public partial class Basictablesforcodinghomework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    CodingTestType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodingHomeworks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodingHomeworkTestRuns",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HomeworkId = table.Column<int>(nullable: false),
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
                        name: "FK_CodingHomeworkTestRuns_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CodingTests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HomeworkId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    InputParameters = table.Column<string>(nullable: true),
                    ExpectedResult = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodingTests", x => x.Id);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_CodingHomeworkTestRuns_UserId",
                table: "CodingHomeworkTestRuns",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodingHomeworks");

            migrationBuilder.DropTable(
                name: "CodingHomeworkTestRuns");

            migrationBuilder.DropTable(
                name: "CodingTests");

            migrationBuilder.DropTable(
                name: "Lectures");
        }
    }
}
