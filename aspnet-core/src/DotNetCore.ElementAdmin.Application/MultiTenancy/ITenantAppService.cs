using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DotNetCore.ElementAdmin.MultiTenancy.Dto;

namespace DotNetCore.ElementAdmin.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

