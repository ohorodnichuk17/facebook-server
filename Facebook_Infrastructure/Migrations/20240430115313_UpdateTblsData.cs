using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Facebook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTblsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Videos",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Posts");

            migrationBuilder.AlterColumn<string>(
                name: "Images",
                table: "Stories",
                type: "text",
                nullable: true,
                oldClrType: typeof(List<string>),
                oldType: "text[]");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Stories",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<string>>(
                name: "Images",
                table: "Stories",
                type: "text[]",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Stories",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "Videos",
                table: "Stories",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Posts",
                type: "text",
                nullable: true);
        }
    }
}
