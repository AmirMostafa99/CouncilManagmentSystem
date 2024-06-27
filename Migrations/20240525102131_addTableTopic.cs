using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CouncilsManagmentSystem.Migrations
{
    public partial class addTableTopic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Councils_Halls_HallId",
                table: "Councils");

            migrationBuilder.AlterColumn<int>(
                name: "HallId",
                table: "Councils",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "topics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDiscussed = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CouncilId = table.Column<int>(type: "int", nullable: false),
                    Attachment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_topics_Councils_CouncilId",
                        column: x => x.CouncilId,
                        principalTable: "Councils",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_topics_CouncilId",
                table: "topics",
                column: "CouncilId");

            migrationBuilder.AddForeignKey(
                name: "FK_Councils_Halls_HallId",
                table: "Councils",
                column: "HallId",
                principalTable: "Halls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Councils_Halls_HallId",
                table: "Councils");

            migrationBuilder.DropTable(
                name: "topics");

            migrationBuilder.AlterColumn<int>(
                name: "HallId",
                table: "Councils",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Councils_Halls_HallId",
                table: "Councils",
                column: "HallId",
                principalTable: "Halls",
                principalColumn: "Id");
        }
    }
}
