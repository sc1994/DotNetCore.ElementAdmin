using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using Newtonsoft.Json;

namespace DotNetCore.ElementAdmin.Models.TokenAuth
{
    public class AuthenticateModel
    {
        [Required]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        [JsonProperty("username")]
        public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        public string Password { get; set; }

        public bool RememberClient { get; set; }
    }
}
