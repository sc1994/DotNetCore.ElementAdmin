using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using DotNetCore.ElementAdmin.Authorization.Roles;
using DotNetCore.ElementAdmin.Authorization.Users;
using DotNetCore.ElementAdmin.MultiTenancy;
using DotNetCore.ElementAdmin.Authorization.Menus;

namespace DotNetCore.ElementAdmin.EntityFrameworkCore
{
    public class ElementAdminDbContext : AbpZeroDbContext<Tenant, Role, User, ElementAdminDbContext>
    {
        /* Define a DbSet for each entity of the application */

        public DbSet<Menu> Menus { get; set; }

        public ElementAdminDbContext(DbContextOptions<ElementAdminDbContext> options)
            : base(options)
        {
        }
    }
}
