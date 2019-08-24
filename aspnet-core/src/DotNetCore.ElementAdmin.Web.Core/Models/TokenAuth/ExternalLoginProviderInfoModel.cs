using Abp.AutoMapper;
using DotNetCore.ElementAdmin.Authentication.External;

namespace DotNetCore.ElementAdmin.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}
