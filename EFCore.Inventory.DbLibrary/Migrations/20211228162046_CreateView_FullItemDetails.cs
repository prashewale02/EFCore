using EFCore.Inventory.DbLibrary.Migrations.Scripts;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.Inventory.DbLibrary.Migrations
{
    public partial class CreateView_FullItemDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.SqlResource("EFCore.Inventory.DbLibrary.Migrations.Scripts.Views.FullItemDetails.FullItemDetails.v0.sql");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[ViewFullItemDetails]");
        }
    }
}
