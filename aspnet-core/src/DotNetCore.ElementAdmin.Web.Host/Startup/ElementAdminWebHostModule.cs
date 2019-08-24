using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using DotNetCore.ElementAdmin.Configuration;

namespace DotNetCore.ElementAdmin.Web.Host.Startup
{
    [DependsOn(
       typeof(ElementAdminWebCoreModule))]
    public class ElementAdminWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public ElementAdminWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ElementAdminWebHostModule).GetAssembly());
        }
    }
}
