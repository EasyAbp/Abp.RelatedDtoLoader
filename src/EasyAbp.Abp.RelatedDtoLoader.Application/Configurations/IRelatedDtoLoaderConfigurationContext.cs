using System;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public interface IRelatedDtoLoaderConfigurationContext
    {
        DtoLoaderConfigurationExpression ConfigurationExpression { get; }
        IServiceProvider ServiceProvider { get; }
    }
}