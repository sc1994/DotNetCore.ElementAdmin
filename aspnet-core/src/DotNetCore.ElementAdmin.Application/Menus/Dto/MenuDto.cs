using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using DotNetCore.ElementAdmin.Authorization.Menus;
using DotNetCore.ElementAdmin.Roles.Dto;

namespace DotNetCore.ElementAdmin.Application.Menus.Dto
{
    [AutoMapFrom(typeof(Menu))]
    [AutoMapTo(typeof(Menu))]
    public class MenuDto : CreationAuditedEntityDto<long>
    {
        [Required]
        public string Key { get; set; }

        public int RoleId { get; set; }
    }
}