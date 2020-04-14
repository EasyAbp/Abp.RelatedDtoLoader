using System;
using EasyAbp.Abp.RelatedDtoLoader.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public class AbpRelatedDtoLoaderModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<IDtoLoaderConfigurationProvider>(CreateDtoLoaderConfiguration);
        }

        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
        }

        private DtoLoaderConfiguration CreateDtoLoaderConfiguration(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<IOptions<RelatedDtoLoaderOptions>>().Value;

            var configuration = new DtoLoaderConfiguration(configurationExpression =>
            {
                var context = new RelatedDtoLoaderConfigurationContext(configurationExpression, serviceProvider);

                foreach (var configurator in options.Configurators)
                {
                    configurator(context);
                }
            });

            return configuration;
        }
    }
}