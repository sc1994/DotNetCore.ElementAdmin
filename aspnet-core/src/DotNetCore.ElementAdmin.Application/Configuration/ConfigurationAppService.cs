using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using DotNetCore.ElementAdmin.Configuration.Dto;

namespace DotNetCore.ElementAdmin.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : ElementAdminAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
