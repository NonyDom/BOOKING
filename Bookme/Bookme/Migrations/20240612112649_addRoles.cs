using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookme.Migrations
{
    /// <inheritdoc />
    public partial class addRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO AspNetRoles VALUES (NEWID(),'Admin','Admin',NEWID());INSERT INTO AspNetRoles VALUES (NEWID(),'SuperAdmin','SuperAdmin',NEWID());");

        }

        /// <inheritdoc />
        
    }
}
