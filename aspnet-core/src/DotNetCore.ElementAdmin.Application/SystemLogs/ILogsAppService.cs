using System;
using System.Threading.Tasks;
using Abp.Application.Services;

namespace DotNetCore.ElementAdmin.Application.SystemLogs
{
    public interface ILogsAppService : IApplicationService
    {
        Task<object> GetAggregation(DateTime startTime, DateTime endTime);
    }
}