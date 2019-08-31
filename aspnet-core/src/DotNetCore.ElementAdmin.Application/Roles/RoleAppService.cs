using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using DotNetCore.ElementAdmin.Authorization;
using DotNetCore.ElementAdmin.Authorization.Roles;
using DotNetCore.ElementAdmin.Authorization.Users;
using DotNetCore.ElementAdmin.Roles.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DotNetCore.ElementAdmin.Authorization.Menus;

namespace DotNetCore.ElementAdmin.Roles
{
    [AbpAuthorize(PermissionNames.Pages_Roles)]
    public class RoleAppService : AsyncCrudAppService<Role, RoleDto, int, PagedRoleResultRequestDto, CreateRoleDto, RoleDto>, IRoleAppService
    {
        private readonly RoleManager _roleManager;
        private readonly UserManager _userManager;
        private readonly MenuManager _menuManager;
        private readonly ILogger<RoleAppService> _log;
        private readonly IRepository<Role> _repository;

        public RoleAppService(
            IRepository<Role> repository,
            RoleManager roleManager,
            UserManager userManager,
            MenuManager menuManager,
            ILogger<RoleAppService> log
        ) : base(repository)
        {
            _repository = repository;
            _roleManager = roleManager;
            _userManager = userManager;
            _menuManager = menuManager;
            _log = log;
        }

        public override async Task<RoleDto> Create(CreateRoleDto input)
        {
            CheckCreatePermission();

            var role = ObjectMapper.Map<Role>(input);
            role.SetNormalizedName();

            CheckErrors(await _roleManager.CreateAsync(role));

            var grantedPermissions = PermissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissions?.Contains(p.Name) ?? false)
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
            await _menuManager.CoverAllAsync(role.Id, input.GrantedMenus);

            return MapToEntityDto(role);
        }

        public async Task<ListResultDto<RoleListDto>> GetRolesAsync(GetRolesInput input)
        {
            var roles = await _roleManager
                .Roles
                .WhereIf(
                    !input.Permission.IsNullOrWhiteSpace(),
                    r => r.Permissions.Any(rp => rp.Name == input.Permission && rp.IsGranted)
                )
                .ToListAsync();

            return new ListResultDto<RoleListDto>(ObjectMapper.Map<List<RoleListDto>>(roles));
        }

        public override async Task<RoleDto> Update(RoleDto input)
        {
            CheckUpdatePermission();

            var role = await _roleManager.GetRoleByIdAsync(input.Id);

            ObjectMapper.Map(input, role);

            CheckErrors(await _roleManager.UpdateAsync(role));

            var grantedPermissions = PermissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissions.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

            await _menuManager.CoverAllAsync(role.Id, input.GrantedMenus);

            return MapToEntityDto(role);
        }

        public override async Task Delete(EntityDto<int> input)
        {
            CheckDeletePermission();

            var role = await _roleManager.FindByIdAsync(input.Id.ToString());
            var users = await _userManager.GetUsersInRoleAsync(role.NormalizedName);

            foreach (var user in users)
            {
                CheckErrors(await _userManager.RemoveFromRoleAsync(user, role.NormalizedName));
            }

            CheckErrors(await _roleManager.DeleteAsync(role));
        }

        public Task<ListResultDto<PermissionDto>> GetAllPermissions()
        {
            _log.LogTrace("Begin-GetAllPermissions");
            _log.LogInformation("Begin-GetAllPermissions");
            var permissions = PermissionManager.GetAllPermissions();

            _log.LogTrace("End-GetAllPermissions");
            _log.LogInformation("End-GetAllPermissions");
            return Task.FromResult(new ListResultDto<PermissionDto>(
                ObjectMapper.Map<List<PermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList()
            ));
        }

        protected override IQueryable<Role> CreateFilteredQuery(PagedRoleResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Permissions)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Keyword)
                || x.DisplayName.Contains(input.Keyword)
                || x.Description.Contains(input.Keyword));
        }

        protected override async Task<Role> GetEntityByIdAsync(int id)
        {
            return await Repository.GetAllIncluding(x => x.Permissions).FirstOrDefaultAsync(x => x.Id == id);
        }

        protected override IQueryable<Role> ApplySorting(IQueryable<Role> query, PagedRoleResultRequestDto input)
        {
            return query.OrderByDescending(x => x.Name == "Admin")
                        .ThenByDescending(r => r.Id);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public async Task<GetRoleForEditOutput> GetRoleForEdit(EntityDto input)
        {
            var permissions = PermissionManager.GetAllPermissions();
            var role = await _roleManager.GetRoleForEdit(input.Id);
            var grantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(role)).ToArray();

            var roleEditDto = ObjectMapper.Map<RoleEditDto>(role);
            roleEditDto.GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList();

            return new GetRoleForEditOutput
            {
                Role = roleEditDto,
                Permissions = ObjectMapper.Map<List<FlatPermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList()
            };
        }
    }
}

