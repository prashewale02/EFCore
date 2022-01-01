CREATE OR ALTER VIEW [dbo].[ViewFullItemDetails]
AS
SELECT item.[Id],
	   item.[Name] ItemName,
	   item.[Description] ItemDescription,
	   item.[IsActive],
	   item.[IsDeleted],
	   item.[Notes],
	   item.[CurrentOrFinalPrice],
	   item.[IsOnSale],
	   item.[PurchasePrice],
	   item.[PurchaseDate],
	   item.[Quantity],
	   item.[SoldDate],
	   cat.[Name] Category,
	   cat.[IsActive] CategoryIsActive,
	   cat.[IsDeleted] CategoryIsDeleted,
	   catDetails.[ColorName],
	   catDetails.[ColorValue],
	   player.[Name] PlayerName,
	   player.[Description] PlayerDescription,
	   player.[IsActive] PlayerIsActive,
	   player.[IsDeleted] PlayerIsDeleted,
	   genre.[Name] GenreName,
	   genre.[IsActive] GenreIsActive,
	   genre.[IsDeleted] GenreIsDeleted
FROM [Items] item
LEFT JOIN [InventoryManagerDb].[dbo].[Categories] cat on item.CategoryId = cat.Id
LEFT JOIN [InventoryManagerDb].[dbo].[CategoryDetails] catDetails on cat.Id = catDetails.Id
LEFT JOIN [InventoryManagerDb].[dbo].[ItemPlayers] itemPlayer on item.Id = itemPlayer.ItemId
LEFT JOIN [InventoryManagerDb].[dbo].[Player] player on itemPlayer.PlayerId = player.Id
LEFT JOIN [InventoryManagerDb].[dbo].[ItemGenres] itemGenres on item.Id = itemGenres.ItemId
LEFT JOIN [InventoryManagerDb].[dbo].[Genres] genre on itemGenres.Id = genre.Id