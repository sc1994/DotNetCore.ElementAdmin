using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DotNetCore.ElementAdmin.Roles.Dto;
using DotNetCore.ElementAdmin.Users.Dto;

namespace DotNetCore.ElementAdmin.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task ChangeLanguage(ChangeUserLanguageDto input);
    }
}
