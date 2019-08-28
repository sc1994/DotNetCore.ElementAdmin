using Abp.Application.Services;
using DotNetCore.ElementAdmin.Application.EfDemos.Dto;

namespace DotNetCore.ElementAdmin.Application.EfDemos
{
    public interface IEfDemoAppService : IAsyncCrudAppService<EfDemoDto, long, GetEfDemoRequest, EfDemoDto, EfDemoDto>
    {

    }
}