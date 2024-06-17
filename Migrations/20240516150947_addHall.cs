using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CouncilsManagmentSystem.Migrations
{
    public partial class addHall : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Halls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfSeats = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Halls", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Councils_HallId",
                table: "Councils",
                column: "HallId");

            migrationBuilder.AddForeignKey(
                name: "FK_Councils_Halls_HallId",
                table: "Councils",
                column: "HallId",
                principalTable: "Halls",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Councils_Halls_HallId",
                table: "Councils");

            migrationBuilder.DropTable(
                name: "Halls");

            migrationBuilder.DropIndex(
                name: "IX_Councils_HallId",
                table: "Councils");
        }
    }
}
