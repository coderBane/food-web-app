using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foody.Data.Migrations
{
    public partial class AddReadField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "read",
                schema: "api",
                table: "inquiries",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "read",
                schema: "api",
                table: "inquiries");
        }
    }
}
