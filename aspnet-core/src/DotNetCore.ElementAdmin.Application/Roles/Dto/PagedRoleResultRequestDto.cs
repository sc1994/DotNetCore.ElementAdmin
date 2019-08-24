using Abp.Application.Services.Dto;

namespace DotNetCore.ElementAdmin.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

