using System;

namespace EasyAbp.Abp.RelatedDtoLoader.Configurations
{
    public interface IRelatedDtoLoaderConfigurationContext
    {
        DtoLoaderConfigurationExpression ConfigurationExpression { get; }
        IServiceProvider ServiceProvider { get; }
    }
}