using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore_Inheritance_Demo_Main9.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TPTCars",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    carName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    carYear = table.Column<int>(type: "int", nullable: false),
                    carPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPTCars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TPTCarModel",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    carModel = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPTCarModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TPTCarModel_TPTCars_Id",
                        column: x => x.Id,
                        principalTable: "TPTCars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TPTCarModel");

            migrationBuilder.DropTable(
                name: "TPTCars");
        }
    }
}
