using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookme.Migrations
{
    /// <inheritdoc />
    public partial class updateBookFrom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BookedUserId",
                table: "BookingForm",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_BookingForm_BookedUserId",
                table: "BookingForm",
                column: "BookedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingForm_AspNetUsers_BookedUserId",
                table: "BookingForm",
                column: "BookedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingForm_AspNetUsers_BookedUserId",
                table: "BookingForm");

            migrationBuilder.DropIndex(
                name: "IX_BookingForm_BookedUserId",
                table: "BookingForm");

            migrationBuilder.AlterColumn<string>(
                name: "BookedUserId",
                table: "BookingForm",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
