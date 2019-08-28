using System;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;

namespace DotNetCore.ElementAdmin.Authorization.Menus
{
    public class MenuManager : DomainService
    {
        private readonly IRepository<Menu, long> _menuRepository;

        public MenuManager(IRepository<Menu, long> menuRepository)
        {
            menuRepository = _menuRepository;
        }

        public Task InsertBatchAsync()
        {
            throw new NotImplementedException();
        }
    }
}