using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using DotNetCore.ElementAdmin.MultiTenancy;

namespace DotNetCore.ElementAdmin.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}
