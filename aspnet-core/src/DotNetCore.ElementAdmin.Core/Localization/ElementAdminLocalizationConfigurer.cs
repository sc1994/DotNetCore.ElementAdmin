using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace DotNetCore.ElementAdmin.Localization
{
    public static class ElementAdminLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(ElementAdminConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(ElementAdminLocalizationConfigurer).GetAssembly(),
                        "DotNetCore.ElementAdmin.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
