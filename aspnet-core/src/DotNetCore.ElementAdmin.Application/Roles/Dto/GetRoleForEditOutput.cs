using System.Collections.Generic;

namespace DotNetCore.ElementAdmin.Roles.Dto
{
    public class GetRoleForEditOutput
    {
        public RoleEditDto Role { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }
    }
}