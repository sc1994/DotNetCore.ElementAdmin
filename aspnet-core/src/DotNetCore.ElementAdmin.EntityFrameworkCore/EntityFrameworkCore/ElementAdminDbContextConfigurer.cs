using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore.ElementAdmin.EntityFrameworkCore
{
    public static class ElementAdminDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<ElementAdminDbContext> builder, string connectionString)
        {
            builder.UseMySql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<ElementAdminDbContext> builder, DbConnection connection)
        {
            builder.UseMySql(connection);
        }
    }
}
