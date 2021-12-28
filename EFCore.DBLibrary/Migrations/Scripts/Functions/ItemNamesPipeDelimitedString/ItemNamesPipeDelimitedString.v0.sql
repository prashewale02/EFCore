CREATE OR ALTER FUNCTION [dbo].[ItemNamesPipeDelimitedString]
(
	-- Add the parameters for the function here
	@IsActive BIT
)
RETURNS VARCHAR (2500)
AS
BEGIN
	RETURN (SELECT STRING_AGG (Name, '|')
				FROM Items
				WHERE IsActive = @IsActive)
END