using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.DBLibrary.AdventureWorks.Migrations
{
    public partial class BackupData_ForTargetColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE [HumanResources].[Employee]
                                 SET [NationalIDNumberBackup] = [NationalIDNumber]
                                 ,[JobTitleBackup] = [JobTitle]
                                 ,[BirthDateBackup] = [BirthDate]
                                 ,[MaritalStatusBackup] = [MaritalStatus]
                                 ,[GenderBackup] = [Gender]
                                 ,[HireDateBackup] = [HireDate]"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
