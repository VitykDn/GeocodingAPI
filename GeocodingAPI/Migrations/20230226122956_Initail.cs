using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeocodingAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddressGeos_CoordinateGeos_CoordinateId",
                table: "AddressGeos");

            migrationBuilder.DropIndex(
                name: "IX_AddressGeos_CoordinateId",
                table: "AddressGeos");

            migrationBuilder.AlterColumn<int>(
                name: "CoordinateId",
                table: "AddressGeos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_AddressGeos_CoordinateId",
                table: "AddressGeos",
                column: "CoordinateId",
                unique: true,
                filter: "[CoordinateId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AddressGeos_CoordinateGeos_CoordinateId",
                table: "AddressGeos",
                column: "CoordinateId",
                principalTable: "CoordinateGeos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddressGeos_CoordinateGeos_CoordinateId",
                table: "AddressGeos");

            migrationBuilder.DropIndex(
                name: "IX_AddressGeos_CoordinateId",
                table: "AddressGeos");

            migrationBuilder.AlterColumn<int>(
                name: "CoordinateId",
                table: "AddressGeos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AddressGeos_CoordinateId",
                table: "AddressGeos",
                column: "CoordinateId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AddressGeos_CoordinateGeos_CoordinateId",
                table: "AddressGeos",
                column: "CoordinateId",
                principalTable: "CoordinateGeos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
