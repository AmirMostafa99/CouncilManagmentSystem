using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CouncilsManagmentSystem.Migrations
{
    public partial class updatePermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AddCollage",
                table: "permissionss",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AddDepartment",
                table: "permissionss",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DeactiveUser",
                table: "permissionss",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UpdateUser",
                table: "permissionss",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Updatepermission",
                table: "permissionss",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddCollage",
                table: "permissionss");

            migrationBuilder.DropColumn(
                name: "AddDepartment",
                table: "permissionss");

            migrationBuilder.DropColumn(
                name: "DeactiveUser",
                table: "permissionss");

            migrationBuilder.DropColumn(
                name: "UpdateUser",
                table: "permissionss");

            migrationBuilder.DropColumn(
                name: "Updatepermission",
                table: "permissionss");
        }
    }
}
