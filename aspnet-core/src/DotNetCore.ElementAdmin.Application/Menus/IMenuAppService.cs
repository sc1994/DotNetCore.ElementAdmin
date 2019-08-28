using Abp.Application.Services;
using DotNetCore.ElementAdmin.Application.Menus.Dto;

namespace DotNetCore.ElementAdmin.Application.Menus
{
    public interface IMenuAppService : IAsyncCrudAppService<MenuDto, long, PagedMenuResultRequestDto, MenuDto, MenuDto>
    {
        
    }
}