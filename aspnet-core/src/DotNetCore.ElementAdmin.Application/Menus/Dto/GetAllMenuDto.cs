using Abp.Application.Services.Dto;

namespace DotNetCore.ElementAdmin.Application.Menus.Dto
{
    public class PagedMenuResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public bool? IsActive { get; set; }
    }
}