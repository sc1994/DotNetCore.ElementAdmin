using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using DotNetCore.ElementAdmin.Authorization.Users;

namespace DotNetCore.ElementAdmin.Sessions.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public List<string> GrantedMenus { get; set; }

        public List<string> GrantedRoles { get; set; }
    }
}
