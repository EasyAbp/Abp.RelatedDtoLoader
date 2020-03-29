using System;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public class RelatedDtoLoaderConfigurationContext : IRelatedDtoLoaderConfigurationContext
    {
        public RelatedDtoLoaderConfigurationContext(
            DtoLoaderConfigurationExpression configurationExpression,
            IServiceProvider serviceProvider)
        {
            ConfigurationExpression = configurationExpression;
            ServiceProvider = serviceProvider;
        }

        public DtoLoaderConfigurationExpression ConfigurationExpression { get; }
        
        public IServiceProvider ServiceProvider { get; }
    }
}