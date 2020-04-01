using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public interface IDtoLoaderConfigurationProvider
    {
        RelatedDtoPropertyCollection GetRelatedDtoProperties(Type targetDtoType);

        IDtoLoadRule GetLoadRule(Type type);
    }
}
