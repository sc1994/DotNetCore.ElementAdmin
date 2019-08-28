using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using DotNetCore.ElementAdmin.Application.Menus.Dto;
using DotNetCore.ElementAdmin.Authorization.Menus;

namespace DotNetCore.ElementAdmin.Application.Menus
{
    public class MenuAppService : AsyncCrudAppService<Menu, MenuDto, long, PagedMenuResultRequestDto, MenuDto, MenuDto>, IMenuAppService
    {
        private readonly IRepository<Menu, long> _repository;

        public MenuAppService(IRepository<Menu, long> repository) : base(repository)
        {
            _repository = repository;
        }


        public override Task<PagedResultDto<MenuDto>> GetAll(PagedMenuResultRequestDto input)
        {
            var debugger = base.GetAll(input);
            return Task.FromResult(new PagedResultDto<MenuDto>
            {
                Items = _repository.GetAll().Select(MapToEntityDto).ToList()
            });
        }
    }
}