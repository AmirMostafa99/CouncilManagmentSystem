using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CouncilsManagmentSystem.Migrations
{
    public partial class addCouncils : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Councils",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HallId = table.Column<int>(type: "int", nullable: true),
                    TypeCouncilId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Councils", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Councils_typeCouncils_TypeCouncilId",
                        column: x => x.TypeCouncilId,
                        principalTable: "typeCouncils",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Councils_TypeCouncilId",
                table: "Councils",
                column: "TypeCouncilId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Councils");
        }
    }
}
