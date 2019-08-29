using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DotNetCore.ElementAdmin.Authorization.Menus
{
    public class MenuManager : DomainService
    {
        private readonly ILogger<MenuManager> _logger;
        private readonly IRepository<Menu, long> _menuRepository;

        public MenuManager(
            IRepository<Menu, long> menuRepository,
            ILogger<MenuManager> logger)
        {
            _menuRepository = menuRepository;
            _logger = logger;
        }

        /// <summary>
        /// 覆盖全部
        /// </summary>
        /// <returns></returns>
        public async Task CoverAllAsync(int roleId, List<string> newMenus)
        {
            await _menuRepository.DeleteAsync(x => x.RoleId == roleId);
            foreach (var menu in newMenus)
            {
                var debugger = await _menuRepository.InsertAndGetIdAsync(new Menu
                {
                    Key = menu,
                    RoleId = roleId
                });
                _logger.LogInformation(JsonConvert.SerializeObject(new { Msg = "test" }));
            }
        }

    }
}