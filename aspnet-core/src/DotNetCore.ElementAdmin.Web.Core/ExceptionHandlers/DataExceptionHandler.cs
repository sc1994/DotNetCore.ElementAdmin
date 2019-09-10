using Abp.Dependency;
using Abp.Events.Bus.Exceptions;
using Abp.Events.Bus.Handlers;
using Microsoft.Extensions.Logging;

namespace DotNetCore.ElementAdmin.Web.Core.ExceptionHandlers
{
    public class DataExceptionHandler : IEventHandler<AbpHandledExceptionData>, ITransientDependency
    {
        private readonly ILogger<DataExceptionHandler> _log;

        public DataExceptionHandler(ILogger<DataExceptionHandler> log)
        {
            log = _log;
        }

        public void HandleEvent(AbpHandledExceptionData eventData)
        {
            // TODO:全局的异常拦截
        }
    }
}