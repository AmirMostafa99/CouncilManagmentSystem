using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CouncilsManagmentSystem.Migrations
{
    public partial class addnotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_departments_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsSeen = table.Column<bool>(type: "bit", nullable: false),
                    MemberId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CouncilId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_CouncilMembers_MemberId_CouncilId",
                        columns: x => new { x.MemberId, x.CouncilId },
                        principalTable: "CouncilMembers",
                        principalColumns: new[] { "MemberId", "CouncilId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_MemberId_CouncilId",
                table: "Notifications",
                columns: new[] { "MemberId", "CouncilId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_departments_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId",
                principalTable: "departments",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_departments_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_departments_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId",
                principalTable: "departments",
                principalColumn: "id");
        }
    }
}
