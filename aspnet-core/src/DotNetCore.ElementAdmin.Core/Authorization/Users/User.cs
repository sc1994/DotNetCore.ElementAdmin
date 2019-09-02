using System.Linq;
using System;
using System.Collections.Generic;
using Abp.Authorization.Users;
using Abp.Extensions;
using DotNetCore.ElementAdmin.Authorization.Roles;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetCore.ElementAdmin.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "123qwe";

        [NotMapped]
        public List<string> GrantedRoles { get; set; }

        [NotMapped]
        public List<string> GrantedMenus { get; set; }

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }
    }
}
