using Abp.Application.Services;
using Abp.Domain.Repositories;
using DotNetCore.ElementAdmin.Application.Menus.Dto;
using DotNetCore.ElementAdmin.EntityFrameworkCore.EntityFrameworkCore.Repositories.RoleExtendRepository;

namespace DotNetCore.ElementAdmin.Application.Menus
{
    public class MenuAppService : AsyncCrudAppService<Menu, MenuDto, long, GetAllMenuDto, MenuDto, MenuDto>, IMenuAppService
    {
        public MenuAppService(IRepository<Menu, long> repository) : base(repository)
        {
        }
    }
}