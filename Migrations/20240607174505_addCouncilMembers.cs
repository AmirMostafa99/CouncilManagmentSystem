using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CouncilsManagmentSystem.Migrations
{
    public partial class addCouncilMembers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CouncilMembers",
                columns: table => new
                {
                    MemberId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CouncilId = table.Column<int>(type: "int", nullable: false),
                    Pdf = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAttending = table.Column<bool>(type: "bit", nullable: false),
                    ReasonNonAttendance = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouncilMembers", x => new { x.MemberId, x.CouncilId });
                    table.ForeignKey(
                        name: "FK_CouncilMembers_AspNetUsers_MemberId",
                        column: x => x.MemberId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CouncilMembers_Councils_CouncilId",
                        column: x => x.CouncilId,
                        principalTable: "Councils",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CouncilMembers_CouncilId",
                table: "CouncilMembers",
                column: "CouncilId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CouncilMembers");
        }
    }
}
