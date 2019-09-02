using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Zero.Configuration;
using DotNetCore.ElementAdmin.Authorization.Users;
using System.Threading.Tasks;

namespace DotNetCore.ElementAdmin.Authorization.Roles
{
    public class RoleManager : AbpRoleManager<Role, User>
    {
        private readonly RoleStore _store;
        public RoleManager(
            RoleStore store,
            IEnumerable<IRoleValidator<Role>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<AbpRoleManager<Role, User>> logger,
            IPermissionManager permissionManager,
            ICacheManager cacheManager,
            IUnitOfWorkManager unitOfWorkManager,
            IRoleManagementConfig roleManagementConfig,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository)
            : base(
                  store,
                  roleValidators,
                  keyNormalizer,
                  errors, logger,
                  permissionManager,
                  cacheManager,
                  unitOfWorkManager,
                  roleManagementConfig,
                  organizationUnitRepository,
                  organizationUnitRoleRepository)
        {
            _store = store;
        }
        
        public async Task<Role> GetRoleForEdit(int key)
        {
            return await _store.GetRoleForEdit(key);
        }
    }
}
