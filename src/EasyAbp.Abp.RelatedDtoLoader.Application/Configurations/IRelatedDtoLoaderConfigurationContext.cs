using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public interface IRelatedDtoLoaderConfigurationContext
    {
        DtoLoaderConfigurationExpression ConfigurationExpression { get; }
        IServiceProvider ServiceProvider { get; }
    }
}
