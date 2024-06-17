using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CouncilsManagmentSystem.Migrations
{
    public partial class DepartemCollegeTypeTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "profilePicture",
                table: "AspNetUsers",
                newName: "img");

            migrationBuilder.RenameColumn(
                name: "FunctionalCharactaristic",
                table: "AspNetUsers",
                newName: "functional_characteristic");

            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "AspNetUsers",
                newName: "Birthday");

            migrationBuilder.RenameColumn(
                name: "AdminstrativeDegree",
                table: "AspNetUsers",
                newName: "administrative_degree");

            migrationBuilder.RenameColumn(
                name: "AcademicDegree",
                table: "AspNetUsers",
                newName: "academic_degree");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "collages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_collages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    collage_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.id);
                    table.ForeignKey(
                        name: "FK_departments_collages_collage_id",
                        column: x => x.collage_id,
                        principalTable: "collages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "typeCouncils",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChairmanCouncilId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SecretaryCouncilId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_typeCouncils", x => x.Id);
                    table.ForeignKey(
                        name: "FK_typeCouncils_AspNetUsers_ChairmanCouncilId",
                        column: x => x.ChairmanCouncilId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                       onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_typeCouncils_AspNetUsers_SecretaryCouncilId",
                        column: x => x.SecretaryCouncilId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_typeCouncils_departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_departments_collage_id",
                table: "departments",
                column: "collage_id");

            migrationBuilder.CreateIndex(
                name: "IX_typeCouncils_ChairmanCouncilId",
                table: "typeCouncils",
                column: "ChairmanCouncilId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_typeCouncils_DepartmentId",
                table: "typeCouncils",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_typeCouncils_SecretaryCouncilId",
                table: "typeCouncils",
                column: "SecretaryCouncilId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_departments_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId",
                principalTable: "departments",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_departments_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "typeCouncils");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "collages");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "img",
                table: "AspNetUsers",
                newName: "profilePicture");

            migrationBuilder.RenameColumn(
                name: "functional_characteristic",
                table: "AspNetUsers",
                newName: "FunctionalCharactaristic");

            migrationBuilder.RenameColumn(
                name: "administrative_degree",
                table: "AspNetUsers",
                newName: "AdminstrativeDegree");

            migrationBuilder.RenameColumn(
                name: "academic_degree",
                table: "AspNetUsers",
                newName: "AcademicDegree");

            migrationBuilder.RenameColumn(
                name: "Birthday",
                table: "AspNetUsers",
                newName: "DateOfBirth");
        }
    }
}
