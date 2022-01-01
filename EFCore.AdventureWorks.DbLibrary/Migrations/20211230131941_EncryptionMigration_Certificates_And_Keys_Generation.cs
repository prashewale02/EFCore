using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.AdventureWorks.DbLibrary.Migrations
{
    public partial class EncryptionMigration_Certificates_And_Keys_Generation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"IF NOT EXISTS ( SELECT *
                FROM sys.symmetric_keys WHERE symmetric_key_id = 101)
                BEGIN
                    CREATE MASTER KEY ENCRYPTION BY PASSWORD = '123654789'
                END");
            migrationBuilder.Sql(
                @"CREATE CERTIFICATE AW_tdeCert
                WITH SUBJECT = 'AdventureWorks TDE Certificate'");
            migrationBuilder.Sql(
                @"BACKUP CERTIFICATE AW_tdeCert TO
                FILE = 'F:\Database\Keys\AW_tdeCert.crt'
                WITH PRIVATE KEY
                (
                    FILE = 'F:\Database\Keys\AW_tdeCert_PrivateKey.crt',
                    ENCRYPTION BY PASSWORD = '123654789'
                )");
            migrationBuilder.Sql(
                @"CREATE SYMMETRIC KEY AW_ColumnKey
                WITH ALGORITHM = AES_256
                ENCRYPTION BY CERTIFICATE AW_tdeCert;
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
