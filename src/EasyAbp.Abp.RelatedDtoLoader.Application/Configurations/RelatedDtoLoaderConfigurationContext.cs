using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public class RelatedDtoLoaderConfigurationContext : IRelatedDtoLoaderConfigurationContext
    {
        public DtoLoaderConfigurationExpression ConfigurationExpression { get; }
        public IServiceProvider ServiceProvider { get; }

        public RelatedDtoLoaderConfigurationContext(
            DtoLoaderConfigurationExpression configurationExpression,
            IServiceProvider serviceProvider)
        {
            ConfigurationExpression = configurationExpression;
            ServiceProvider = serviceProvider;
        }
    }
}
