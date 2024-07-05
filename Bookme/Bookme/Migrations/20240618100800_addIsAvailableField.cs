using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookme.Migrations
{
    /// <inheritdoc />
    public partial class addIsAvailableField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "AspNetUsers");
        }
    }
}
