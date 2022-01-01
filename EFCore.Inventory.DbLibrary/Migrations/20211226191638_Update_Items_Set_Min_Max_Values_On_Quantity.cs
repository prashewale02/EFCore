using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.Inventory.DbLibrary.Migrations
{
    public partial class Update_Items_Set_Min_Max_Values_On_Quantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Make sure no data exists in the table that is outside of the constraint values.
            migrationBuilder.Sql("UPDATE [InventoryManagerDb].[dbo].[Items] SET Quantity = 0 WHERE Quantity < 0");
            migrationBuilder.Sql("UPDATE [InventoryManagerDb].[dbo].[Items] SET Quantity = 1000 WHERE Quantity > 1000");
            
            // Add constraint on Quantity
            migrationBuilder.Sql(
                    @"IF NOT EXISTS ( SELECT *
                        FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
                        WHERE CONSTRAINT_NAME = 'CK_Items_Quantity_Minimum' )
                    BEGIN
                        ALTER TABLE [InventoryManagerDb].[dbo].[Items] 
                        ADD CONSTRAINT CK_Items_Quantity_Minimum
                        CHECK ( Quantity >= 0 )
                    END

                    IF NOT EXISTS ( SELECT *
                        FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
                        WHERE CONSTRAINT_NAME = 'CK_Items_Quantity_Maximum' )
                    BEGIN
                        ALTER TABLE [InventoryManagerDb].[dbo].[Items]
                        ADD CONSTRAINT CK_Items_Quantity_Maximum
                        CHECK ( Quantity <= 1000 )
                    END"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Rollback Constraints on Quantity
            migrationBuilder.Sql(
                @"IF EXISTS ( SELECT *
                    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
                    WHERE CONSTRAINT_NAME = 'CK_Items_Quantity_Minimum' )
                BEGIN
                    ALTER TABLE [InventoryManagerDb].[dbo].[Items]
                    DROP CONSTRAINT Ck_Items_Quantity_Minimum
                END
                
                IF EXISTS ( SELECT *
                    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
                    WHERE CONSTRAINT_NAME = 'Ck_Items_Quantity_Maximum' )
                BEGIN
                    ALTER TABLE [InventoryManagerDb].[dbo].[Items]
                    DROP CONSTRAINT Ck_Items_Quantity_Maximum
                END"
            );
        }
    }
}
