using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Facebook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubActioninPostEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SubActionId",
                table: "Posts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_SubActionId",
                table: "Posts",
                column: "SubActionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_SubActions_SubActionId",
                table: "Posts",
                column: "SubActionId",
                principalTable: "SubActions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_SubActions_SubActionId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_SubActionId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "SubActionId",
                table: "Posts");
        }
    }
}
