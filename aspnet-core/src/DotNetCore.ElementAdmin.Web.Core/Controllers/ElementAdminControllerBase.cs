using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace DotNetCore.ElementAdmin.Controllers
{
    public abstract class ElementAdminControllerBase: AbpController
    {
        protected ElementAdminControllerBase()
        {
            LocalizationSourceName = ElementAdminConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
