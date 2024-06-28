using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Facebook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addReactionEntityTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Emoji = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostEntityReactionEntity",
                columns: table => new
                {
                    PostsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReactionsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostEntityReactionEntity", x => new { x.PostsId, x.ReactionsId });
                    table.ForeignKey(
                        name: "FK_PostEntityReactionEntity_Posts_PostsId",
                        column: x => x.PostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostEntityReactionEntity_Reactions_ReactionsId",
                        column: x => x.ReactionsId,
                        principalTable: "Reactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Reactions",
                columns: new[] { "Id", "Code", "Emoji" },
                values: new object[,]
                {
                    { new Guid("277bf060-ef5e-4fe3-b19e-e6cba28d1bcb"), ":sad:", "😭" },
                    { new Guid("2fb18f6b-7483-4f4f-9348-1eca2dc333f2"), ":smart:", "🤓" },
                    { new Guid("9261147d-b5c3-4441-bbf6-44d8422ebdb3"), ":angry:", "🤬" },
                    { new Guid("c4391285-e3df-4a78-8862-0ed1c3559f5a"), ":clown:", "🤡" },
                    { new Guid("e947b1e8-dd6f-496f-97ec-0b523e15f95f"), ":like:", "👍" },
                    { new Guid("e96309a8-f13f-4714-ad6f-7b922e1cf42f"), ":love:", "❤️" },
                    { new Guid("ed6cd369-0bfa-4822-9ea6-d3e789b4a4c5"), ":wow:", "😮" },
                    { new Guid("f76bb596-1600-476c-83c3-ee69c2c98b9a"), ":haha:", "🤣" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostEntityReactionEntity_ReactionsId",
                table: "PostEntityReactionEntity",
                column: "ReactionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostEntityReactionEntity");

            migrationBuilder.DropTable(
                name: "Reactions");
        }
    }
}
