using System.Collections.Generic;

namespace DotNetCore.ElementAdmin.Authentication.External
{
    public interface IExternalAuthConfiguration
    {
        List<ExternalLoginProviderInfo> Providers { get; }
    }
}
