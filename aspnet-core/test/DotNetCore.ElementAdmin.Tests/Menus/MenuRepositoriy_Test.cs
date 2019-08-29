using System.Diagnostics;
using Abp.Domain.Repositories;
using DotNetCore.ElementAdmin.Authorization.Menus;
using DotNetCore.ElementAdmin.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace DotNetCore.ElementAdmin.Tests.Menus
{
    public class MenuRepositoriy_Test : ElementAdminTestBase
    {
        private readonly IRepository<Menu, long> _menuRepository;
        private readonly ElementAdminDbContext _baseRepository;

        public MenuRepositoriy_Test()
        {
            _menuRepository = Resolve<IRepository<Menu, long>>();
            _baseRepository = Resolve<ElementAdminDbContext>();
        }

        [Fact]
        public void Get_Menu_Roles_Test()
        {
            // var result = _menuRepository.GetAll();

            // Debug.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            var result1 = _baseRepository.Menus
                                         .Include(x => x.Role)
                                         .ToListAsync()
                                         .Result;
            Debug.WriteLine(JsonConvert.SerializeObject(result1, Formatting.Indented));
        }
    }
}