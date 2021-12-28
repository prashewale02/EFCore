using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.DBLibrary.Migrations
{
    public partial class UpdateItems_Change_IsDeletable_Column_To_IsDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeletable",
                table: "Player",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsDeletable",
                table: "Items",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsDeletable",
                table: "Genres",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsDeletable",
                table: "Categories",
                newName: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Player",
                newName: "IsDeletable");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Items",
                newName: "IsDeletable");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Genres",
                newName: "IsDeletable");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Categories",
                newName: "IsDeletable");
        }
    }
}
