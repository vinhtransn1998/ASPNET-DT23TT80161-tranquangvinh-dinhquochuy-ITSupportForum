using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITSupportForum.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Post",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Post");
        }
    }
}
