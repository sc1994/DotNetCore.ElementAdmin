using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using DotNetCore.ElementAdmin.Authorization;

namespace DotNetCore.ElementAdmin
{
    [DependsOn(
        typeof(ElementAdminCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class ElementAdminApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<ElementAdminAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(ElementAdminApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
        }
    }
}
