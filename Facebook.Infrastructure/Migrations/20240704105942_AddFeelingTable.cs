using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Facebook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFeelingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FeelingId",
                table: "Posts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Feelings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Emoji = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feelings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_FeelingId",
                table: "Posts",
                column: "FeelingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Feelings_FeelingId",
                table: "Posts",
                column: "FeelingId",
                principalTable: "Feelings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Feelings_FeelingId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "Feelings");

            migrationBuilder.DropIndex(
                name: "IX_Posts_FeelingId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "FeelingId",
                table: "Posts");
        }
    }
}
