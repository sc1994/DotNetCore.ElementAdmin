using Abp.MultiTenancy;
using DotNetCore.ElementAdmin.Authorization.Users;

namespace DotNetCore.ElementAdmin.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
