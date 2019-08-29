using System.Diagnostics;
using Abp.Domain.Repositories;
using DotNetCore.ElementAdmin.Authorization.Roles;
using Newtonsoft.Json;
using Xunit;

namespace DotNetCore.ElementAdmin.Tests.Roles
{
    public class RoleRepositoriy_Test : ElementAdminTestBase
    {
        private readonly IRepository<Role> _roleRepository;

        public RoleRepositoriy_Test()
        {
            _roleRepository = Resolve<IRepository<Role>>();
        }

        [Fact]
        public void Get_Role_Menus_Test()
        {
            var result = _roleRepository.GetAll();
            Debug.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }
    }
}