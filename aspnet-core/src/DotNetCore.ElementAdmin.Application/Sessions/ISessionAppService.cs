using System.Threading.Tasks;
using Abp.Application.Services;
using DotNetCore.ElementAdmin.Sessions.Dto;

namespace DotNetCore.ElementAdmin.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
