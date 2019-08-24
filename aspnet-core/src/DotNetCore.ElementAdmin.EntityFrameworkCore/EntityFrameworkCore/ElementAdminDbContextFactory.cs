using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using DotNetCore.ElementAdmin.Configuration;
using DotNetCore.ElementAdmin.Web;

namespace DotNetCore.ElementAdmin.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class ElementAdminDbContextFactory : IDesignTimeDbContextFactory<ElementAdminDbContext>
    {
        public ElementAdminDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ElementAdminDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            ElementAdminDbContextConfigurer.Configure(builder, configuration.GetConnectionString(ElementAdminConsts.ConnectionStringName));

            return new ElementAdminDbContext(builder.Options);
        }
    }
}
