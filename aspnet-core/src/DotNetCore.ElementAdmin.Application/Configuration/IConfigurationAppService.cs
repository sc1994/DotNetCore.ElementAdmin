using System.Threading.Tasks;
using DotNetCore.ElementAdmin.Configuration.Dto;

namespace DotNetCore.ElementAdmin.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
