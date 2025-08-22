using System;
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
                name: "LogInfoes",
                columns: table => new
                {
                    LogInfoId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogInfoUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogInfoDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogInfoDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogInfoes", x => x.LogInfoId);
                });

            migrationBuilder.CreateTable(
                name: "TPTCars",
                columns: table => new
                {
                    carId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    carName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    carYear = table.Column<int>(type: "int", nullable: false),
                    carPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPTCars", x => x.carId);
                });

            migrationBuilder.CreateTable(
                name: "TPTCarModel",
                columns: table => new
                {
                    carId = table.Column<long>(type: "bigint", nullable: false),
                    carModel = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPTCarModel", x => x.carId);
                    table.ForeignKey(
                        name: "FK_TPTCarModel_TPTCars_carId",
                        column: x => x.carId,
                        principalTable: "TPTCars",
                        principalColumn: "carId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogInfoes");

            migrationBuilder.DropTable(
                name: "TPTCarModel");

            migrationBuilder.DropTable(
                name: "TPTCars");
        }
    }
}
