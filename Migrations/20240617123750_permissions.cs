using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CouncilsManagmentSystem.Migrations
{
    public partial class permissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "permissionss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddCouncil = table.Column<bool>(type: "bit", nullable: false),
                    EditCouncil = table.Column<bool>(type: "bit", nullable: false),
                    CreateTypeCouncil = table.Column<bool>(type: "bit", nullable: false),
                    EditTypeCouncil = table.Column<bool>(type: "bit", nullable: false),
                    AddMembersByExcil = table.Column<bool>(type: "bit", nullable: false),
                    AddMembers = table.Column<bool>(type: "bit", nullable: false),
                    AddTopic = table.Column<bool>(type: "bit", nullable: false),
                    Arrange = table.Column<bool>(type: "bit", nullable: false),
                    AddResult = table.Column<bool>(type: "bit", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissionss", x => x.Id);
                    table.ForeignKey(
                        name: "FK_permissionss_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_permissionss_userId",
                table: "permissionss",
                column: "userId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "permissionss");
        }
    }
}
