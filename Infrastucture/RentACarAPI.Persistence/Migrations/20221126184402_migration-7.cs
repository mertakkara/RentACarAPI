using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACarAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class migration7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
