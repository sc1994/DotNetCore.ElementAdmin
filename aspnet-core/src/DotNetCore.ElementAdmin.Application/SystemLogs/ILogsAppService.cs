using System.Threading.Tasks;
using Abp.Application.Services;
using System.Collections.Generic;
using DotNetCore.ElementAdmin.SystemLogs.Dto;

namespace DotNetCore.ElementAdmin.Application.SystemLogs
{
    public interface ILogsAppService : IApplicationService
    {
        Task<Dictionary<string, object>> PostAggregation(LogFiltrateInputDto input);

        Task<string> PostSearch(LogFiltrateInputDto input);

        Task<List<string>> GetAllIndexTimes();
    }
}