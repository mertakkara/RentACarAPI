using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACarAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class migration8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Cars_CarId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_CarId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Files");

            migrationBuilder.CreateTable(
                name: "CarCarImageFile",
                columns: table => new
                {
                    CarImageFilesId = table.Column<Guid>(type: "uuid", nullable: false),
                    CarsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarCarImageFile", x => new { x.CarImageFilesId, x.CarsId });
                    table.ForeignKey(
                        name: "FK_CarCarImageFile_Cars_CarsId",
                        column: x => x.CarsId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarCarImageFile_Files_CarImageFilesId",
                        column: x => x.CarImageFilesId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarCarImageFile_CarsId",
                table: "CarCarImageFile",
                column: "CarsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarCarImageFile");

            migrationBuilder.AddColumn<Guid>(
                name: "CarId",
                table: "Files",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_CarId",
                table: "Files",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Cars_CarId",
                table: "Files",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
