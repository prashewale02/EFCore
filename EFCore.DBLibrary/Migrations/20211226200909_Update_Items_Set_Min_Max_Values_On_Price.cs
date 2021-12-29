using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.DBLibrary.InventoryManager.Migrations
{
    public partial class Update_Items_Set_Min_Max_Values_On_Price : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE [InventoryManagerDb].[dbo].[Items] 
                SET PurchasePrice = 0 WHERE PurchasePrice < 0");

            migrationBuilder.Sql(@"UPDATE [InventoryManagerDb].[dbo].[Items]
                SET PurchasePrice = 25000 WHERE PurchasePrice > 25000");
            migrationBuilder.Sql(
                @"IF NOT EXISTS ( SELECT *
	                FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
	                WHERE CONSTRAINT_NAME = 'CK_Items_PurchasePrice_Minimum' )
                BEGIN
	                ALTER TABLE [InventoryManagerDb].[dbo].[Items]
	                ADD CONSTRAINT Ck_Items_PurchasePrice_Minimum
                    CHECK ( PurchasePrice >= 0 )
                END

                IF NOT EXISTS ( SELECT *
	                FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
	                WHERE CONSTRAINT_NAME = 'Ck_Items_PurchasePrice_Maximum' )
                BEGIN
	                ALTER TABLE [InventoryManagerDb].[dbo].[Items]
	                ADD CONSTRAINT Ck_Items_PurchasPrice_Maximum
                    CHECK ( PurchasePrice <= 25000 )
                END"
            );

            migrationBuilder.Sql(@"UPDATE [InventoryManagerDb].[dbo].[Items] 
                SET CurrentOrFinalPrice = 0 WHERE CurrentOrFinalPrice < 0");

            migrationBuilder.Sql(@"UPDATE [InventoryManagerDb].[dbo].[Items]
                SET CurrentOrFinalPrice = 25000 WHERE CurrentOrFinalPrice > 25000");
            migrationBuilder.Sql(
                @"IF NOT EXISTS ( SELECT *
	                FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
	                WHERE CONSTRAINT_NAME = 'CK_Items_CurrentOrFinalPrice_Minimum' )
                BEGIN
	                ALTER TABLE [InventoryManagerDb].[dbo].[Items]
	                ADD CONSTRAINT Ck_Items_CurrentOrFinalPrice_Minimum
                    CHECK ( CurrentOrFinalPrice >= 0 )
                END

                IF NOT EXISTS ( SELECT *
	                FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
	                WHERE CONSTRAINT_NAME = 'Ck_Items_CurrentOrFinalPrice_Maximum' )
                BEGIN
	                ALTER TABLE [InventoryManagerDb].[dbo].[Items]
	                ADD CONSTRAINT Ck_Items_CurrentOrFinalPrice_Maximum
                    CHECK ( CurrentOrFinalPrice <= 25000 )
                END"
            );

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"IF EXISTS ( SELECT *
	                FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
	                WHERE CONSTRAINT_NAME = 'CK_Items_PurchasePrice_Minimum' )
                BEGIN
	                ALTER TABLE [InventoryManagerDb].[dbo].[Items]
	                DROP CONSTRAINT Ck_Items_PurchasePrice_Minimum
                END

                IF EXISTS ( SELECT *
	                FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
	                WHERE CONSTRAINT_NAME = 'Ck_Items_PurchasePrice_Maximum' )
                BEGIN
	                ALTER TABLE [InventoryManagerDb].[dbo].[Items]
	                DROP CONSTRAINT Ck_Items_PurchasPrice_Maximum
                END"
            );
            
            migrationBuilder.Sql(
                @"IF EXISTS ( SELECT *
	                FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
	                WHERE CONSTRAINT_NAME = 'CK_Items_CurrentOrFinalPrice_Minimum' )
                BEGIN
	                ALTER TABLE [InventoryManagerDb].[dbo].[Items]
	                DROP CONSTRAINT Ck_Items_CurrentOrFinalPrice_Minimum
                END

                IF EXISTS ( SELECT *
	                FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
	                WHERE CONSTRAINT_NAME = 'Ck_Items_CurrentOrFinalPrice_Maximum' )
                BEGIN
	                ALTER TABLE [InventoryManagerDb].[dbo].[Items]
	                DROP CONSTRAINT Ck_Items_CurrentOrFinalPrice_Maximum
                END"
            );
        }
    }
}
