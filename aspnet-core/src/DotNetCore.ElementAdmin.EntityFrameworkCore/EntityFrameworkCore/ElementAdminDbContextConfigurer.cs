using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore.ElementAdmin.EntityFrameworkCore
{
    public static class ElementAdminDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<ElementAdminDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<ElementAdminDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
