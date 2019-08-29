using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Authorization.Roles;
using DotNetCore.ElementAdmin.Authorization.Menus;
using DotNetCore.ElementAdmin.Authorization.Users;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore.ElementAdmin.Authorization.Roles
{
    public class Role : AbpRole<User>
    {
        public const int MaxDescriptionLength = 5000;

        public List<Menu> Menus { get; set; }

        public Role()
        {

        }

        public Role(int? tenantId, string displayName)
            : base(tenantId, displayName)
        {
        }

        public Role(int? tenantId, string name, string displayName)
            : base(tenantId, name, displayName)
        {
        }

        [StringLength(MaxDescriptionLength)]
        public string Description { get; set; }
    }
}
