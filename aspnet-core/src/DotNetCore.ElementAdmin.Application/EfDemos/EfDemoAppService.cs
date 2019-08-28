using Abp.Application.Services;
using Abp.Domain.Repositories;
using DotNetCore.ElementAdmin.Application.EfDemos.Dto;
using DotNetCore.ElementAdmin.Core.EfDemos;

namespace DotNetCore.ElementAdmin.Application.EfDemos
{
    public class EfDemoAppService : AsyncCrudAppService<EfDemo, EfDemoDto, long, GetEfDemoRequest, EfDemoDto, EfDemoDto>, IEfDemoAppService
    {
        public EfDemoAppService(IRepository<EfDemo, long> repository) : base(repository)
        {
        }
    }
}