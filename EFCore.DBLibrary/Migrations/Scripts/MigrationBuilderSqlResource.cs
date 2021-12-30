using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using System.Reflection;
using System.Text;

namespace EFCore.DBLibrary.InventoryManager.Migrations.Scripts
{
    public static class MigrationBuilderSqlResource
    {
        public static OperationBuilder<SqlOperation> SqlResource(
            this MigrationBuilder mb, string relativeFileName)
        {
            var assembly = Assembly.GetAssembly(typeof(MigrationBuilderSqlResource));

            if ( assembly is not null)
            {
                using (Stream? stream = 
                            assembly.GetManifestResourceStream(relativeFileName))
                {
                    if( stream is not null)
                    {
                        using (var ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            var data = ms.ToArray();
                            var text = Encoding.UTF8.GetString(data, 3,
                                data.Length - 3);
                            return mb.Sql(text);
                        }
                    }

                    throw new Exception("File Not Found!");
                }
            }

            throw new Exception("Assembly is Null!");
        }
    }
}
