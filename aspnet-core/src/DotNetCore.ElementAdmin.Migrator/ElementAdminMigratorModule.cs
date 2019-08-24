using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using DotNetCore.ElementAdmin.Configuration;
using DotNetCore.ElementAdmin.EntityFrameworkCore;
using DotNetCore.ElementAdmin.Migrator.DependencyInjection;

namespace DotNetCore.ElementAdmin.Migrator
{
    [DependsOn(typeof(ElementAdminEntityFrameworkModule))]
    public class ElementAdminMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public ElementAdminMigratorModule(ElementAdminEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(ElementAdminMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                ElementAdminConsts.ConnectionStringName
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus), 
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ElementAdminMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
