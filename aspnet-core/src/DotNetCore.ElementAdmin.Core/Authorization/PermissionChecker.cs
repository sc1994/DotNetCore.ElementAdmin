using Abp.Authorization;
using DotNetCore.ElementAdmin.Authorization.Roles;
using DotNetCore.ElementAdmin.Authorization.Users;

namespace DotNetCore.ElementAdmin.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
