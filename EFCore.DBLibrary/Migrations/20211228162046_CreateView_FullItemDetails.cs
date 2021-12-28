using EFCore.DBLibrary.Migrations.Scripts;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.DBLibrary.Migrations
{
    public partial class CreateView_FullItemDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.SqlResource("EFCore.DBLibrary.Migrations.Scripts.Views.FullItemDetails.FullItemDetails.v0.sql");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[ViewFullItemDetails]");
        }
    }
}
