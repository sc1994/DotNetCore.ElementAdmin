using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Roles;
using DotNetCore.ElementAdmin.Authorization.Menus;
using DotNetCore.ElementAdmin.Authorization.Users;

namespace DotNetCore.ElementAdmin.Authorization.Roles
{
    public class Role : AbpRole<User>
    {
        public const int MaxDescriptionLength = 5000;

        public ICollection<Menu> Menus { get; set; }

        public Role()
        {
            Menus = new HashSet<Menu>();
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
