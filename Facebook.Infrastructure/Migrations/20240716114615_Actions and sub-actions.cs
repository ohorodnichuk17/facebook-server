using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Facebook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Actionsandsubactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Feelings_FeelingId",
                table: "Posts");

            migrationBuilder.AddColumn<Guid>(
                name: "ActionId",
                table: "Posts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Emoji = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ActionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubActions_Actions_ActionId",
                        column: x => x.ActionId,
                        principalTable: "Actions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ActionId",
                table: "Posts",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubActions_ActionId",
                table: "SubActions",
                column: "ActionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Actions_ActionId",
                table: "Posts",
                column: "ActionId",
                principalTable: "Actions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Feelings_FeelingId",
                table: "Posts",
                column: "FeelingId",
                principalTable: "Feelings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Actions_ActionId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Feelings_FeelingId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "SubActions");

            migrationBuilder.DropTable(
                name: "Actions");

            migrationBuilder.DropIndex(
                name: "IX_Posts_ActionId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ActionId",
                table: "Posts");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Feelings_FeelingId",
                table: "Posts",
                column: "FeelingId",
                principalTable: "Feelings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
