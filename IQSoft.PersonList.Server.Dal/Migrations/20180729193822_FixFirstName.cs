using Microsoft.EntityFrameworkCore.Migrations;

namespace IQSoft.PersonList.Server.Dal.Migrations
{
    public partial class FixFirstName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FisrtName",
                table: "Persons",
                newName: "FirstName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Persons",
                newName: "FisrtName");
        }
    }
}
