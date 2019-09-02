using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq;
using Abp.Organizations;
using DotNetCore.ElementAdmin.Authorization.Roles;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore.ElementAdmin.Authorization.Users
{
    public class UserStore : AbpUserStore<Role, User>
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Role> _roleRepository;

        public UserStore(
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<User, long> userRepository,
            IRepository<Role> roleRepository,
            IAsyncQueryableExecuter asyncQueryableExecuter,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<UserLogin, long> userLoginRepository,
            IRepository<UserClaim, long> userClaimRepository,
            IRepository<UserPermissionSetting, long> userPermissionSettingRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository)
            : base(
                unitOfWorkManager,
                userRepository,
                roleRepository,
                asyncQueryableExecuter,
                userRoleRepository,
                userLoginRepository,
                userClaimRepository,
                userPermissionSettingRepository,
                userOrganizationUnitRepository,
                organizationUnitRoleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<User> GetFullUserByIdAsync(string id)
        {
            // DbSet
            var user = await _userRepository.GetAll()
                                            .Include(x =>
                                                x.Roles
                                            )
                                            .FirstOrDefaultAsync(x => x.Id == int.Parse(id));

            var roleIds = user.Roles.Select(s => s.Id).ToList();
            var roles = await _roleRepository.GetAll()
                                             .Include(x => x.Menus)
                                             .Select(s => new
                                             {
                                                 s.Menus,
                                                 s.Id,
                                                 s.NormalizedName
                                             })
                                             .Where(x => roleIds.Contains(x.Id))
                                             .ToListAsync();

            user.GrantedMenus = roles.SelectMany(
                                          x => x.Menus.Select(s => s.Key)
                                      ).ToList();
            user.GrantedRoles = roles.Select(x => x.NormalizedName).ToList();
            return user;
        }
    }
}
