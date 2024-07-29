using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Facebook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateposttableaddprivacySettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid[]>(
                name: "ExcludedFriends",
                table: "Posts",
                type: "uuid[]",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Visibility",
                table: "Posts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExcludedFriends",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Visibility",
                table: "Posts");
        }
    }
}
