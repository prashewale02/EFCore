using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.Inventory.DbLibrary.Migrations
{
    public partial class Add_Soft_Delete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeletable",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeletable",
                table: "Items");
        }
    }
}
