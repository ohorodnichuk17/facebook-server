using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Facebook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureEntitiesUsingFluentAPI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stories_AspNetUsers_UserEntityId",
                table: "Stories");

            migrationBuilder.DropIndex(
                name: "IX_UsersProfiles_UserId",
                table: "UsersProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Stories_UserEntityId",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "Stories");

            migrationBuilder.CreateIndex(
                name: "IX_UsersProfiles_UserId",
                table: "UsersProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stories_UserId",
                table: "Stories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_AspNetUsers_UserId",
                table: "Stories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stories_AspNetUsers_UserId",
                table: "Stories");

            migrationBuilder.DropIndex(
                name: "IX_UsersProfiles_UserId",
                table: "UsersProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Stories_UserId",
                table: "Stories");

            migrationBuilder.AddColumn<Guid>(
                name: "UserEntityId",
                table: "Stories",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersProfiles_UserId",
                table: "UsersProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_UserEntityId",
                table: "Stories",
                column: "UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_AspNetUsers_UserEntityId",
                table: "Stories",
                column: "UserEntityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
