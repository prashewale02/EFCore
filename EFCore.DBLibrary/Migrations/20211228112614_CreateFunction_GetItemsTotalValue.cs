using EFCore.DBLibrary.InventoryManager.Migrations.Scripts;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.DBLibrary.InventoryManager.Migrations
{
    public partial class CreateFunction_GetItemsTotalValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.SqlResource("EFCore.DBLibrary.InventoryManager.Migrations.Scripts.Functions.GetItemsTotalValue.GetItemsTotalValue.v0.sql");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[GetItemsTotalValue]");
        }
    }
}
