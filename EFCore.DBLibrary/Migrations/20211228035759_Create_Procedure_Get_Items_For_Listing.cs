using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.DBLibrary.InventoryManager.Migrations
{
    public partial class Create_Procedure_Get_Items_For_Listing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE OR ALTER PROCEDURE dbo.GetItemsForListing
                    @minDate DATETIME = '1970.01.01',
                    @maxDate DATETIME = '2050.12.31'

                AS
                BEGIN
                    SET NOCOUNT ON;
                
                    SELECT 
	                    item.[Name], 
	                    item.[Description],
	                    item.[Notes],
	                    item.[IsActive],
	                    item.[IsDeletable],
	                    genre.[Name] GenreName,
	                    cat.[Name] CategoryName
                    FROM [InventoryManagerDb].[dbo].[Items] item
                    LEFT JOIN [InventoryManagerDb].[dbo].[ItemGenres] ig on item.Id = ig.ItemId
                    LEFT JOIN [InventoryManagerDb].[dbo].[Genres] genre on ig.GenreId = genre.Id
                    LEFT JOIN [InventoryManagerDb].[dbo].[Categories] cat on item.CategoryId = cat.Id
                    WHERE (@minDate IS NULL OR item.CreatedDate >= @minDate)
                    AND (@maxDate IS NULL OR item.CreatedDate <= @maxDate)
                END
                GO"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"DROP PROCEDURE IF EXISTS [dbo].[GetItemsForListing]");
        }
    }
}
