using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.DBLibrary.InventoryManager.Migrations
{
    public partial class DataUpdate_SeedGenresMigration_CategoriesInInventoryMigrator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "CreatedByUserId", "CreatedDate", "IsActive", "IsDeleted", "LastModifiedDate", "LastModifiedUserId", "Name" },
                values: new object[,]
                {
                    { 1, "2fd28110-93d0-427d-9207-d55dbca680fa", new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, null, null, "Fantasy" },
                    { 2, "2fd28110-93d0-427d-9207-d55dbca680fa", new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, null, null, "Sci/Fi" },
                    { 3, "2fd28110-93d0-427d-9207-d55dbca680fa", new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, null, null, "Horror" },
                    { 4, "2fd28110-93d0-427d-9207-d55dbca680fa", new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, null, null, "Comedy" },
                    { 5, "2fd28110-93d0-427d-9207-d55dbca680fa", new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, null, null, "Drama" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
