using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestData.Migrations
{
    /// <inheritdoc />
    public partial class ChangedUser1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfirmationToken",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailConfirmed",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenExpirationDate",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ConfirmationToken",
                table: "Users",
                column: "ConfirmationToken",
                unique: true,
                filter: "[ConfirmationToken] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_ConfirmationToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ConfirmationToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsEmailConfirmed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TokenExpirationDate",
                table: "Users");
        }
    }
}
