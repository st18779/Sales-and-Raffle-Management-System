using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class migration6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "purchases",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchaseDate",
                table: "purchases",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.CreateIndex(
                name: "IX_purchases_GiftId",
                table: "purchases",
                column: "GiftId");

            migrationBuilder.CreateIndex(
                name: "IX_purchases_UserId",
                table: "purchases",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_purchases_Gifts_GiftId",
                table: "purchases",
                column: "GiftId",
                principalTable: "Gifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_purchases_Users_UserId",
                table: "purchases",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_purchases_Gifts_GiftId",
                table: "purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_purchases_Users_UserId",
                table: "purchases");

            migrationBuilder.DropIndex(
                name: "IX_purchases_GiftId",
                table: "purchases");

            migrationBuilder.DropIndex(
                name: "IX_purchases_UserId",
                table: "purchases");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "purchases",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchaseDate",
                table: "purchases",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
