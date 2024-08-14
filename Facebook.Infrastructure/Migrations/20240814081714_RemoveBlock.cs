using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Facebook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBlock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "UsersProfiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "UsersProfiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
