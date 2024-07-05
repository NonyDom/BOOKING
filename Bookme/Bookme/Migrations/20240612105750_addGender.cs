using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookme.Migrations
{
    /// <inheritdoc />
    public partial class addGender : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO CommonDropown VALUES ('Female',GETDATE(),1,0);INSERT INTO CommonDropown VALUES ('Male',GETDATE(),1,0);");
        }

        /// <inheritdoc />
       
    }
}
