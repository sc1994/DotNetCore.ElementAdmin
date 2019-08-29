using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Roles;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using DotNetCore.ElementAdmin.Authorization.Users;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore.ElementAdmin.Authorization.Roles
{
    public class RoleStore : AbpRoleStore<Role, User>
    {
        private readonly IRepository<Role> _roleRepository;
        public RoleStore(
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<Role> roleRepository,
            IRepository<RolePermissionSetting, long> rolePermissionSettingRepository)
            : base(
                unitOfWorkManager,
                roleRepository,
                rolePermissionSettingRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Role> GetRoleForEdit(int key)
        {
            var result = await _roleRepository
                                .GetAll()
                                .Include(x => x.Menus)
                                .FirstOrDefaultAsync(x => x.Id == key);
            return result;
        }
    }
}
