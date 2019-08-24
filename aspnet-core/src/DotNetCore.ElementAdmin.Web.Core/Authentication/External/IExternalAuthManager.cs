using System.Threading.Tasks;

namespace DotNetCore.ElementAdmin.Authentication.External
{
    public interface IExternalAuthManager
    {
        Task<bool> IsValidUser(string provider, string providerKey, string providerAccessCode);

        Task<ExternalAuthUserInfo> GetUserInfo(string provider, string accessCode);
    }
}
