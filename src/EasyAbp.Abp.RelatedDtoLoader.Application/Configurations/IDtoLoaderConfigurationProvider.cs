using System;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public interface IDtoLoaderConfigurationProvider
    {
        RelatedDtoPropertyCollection GetRelatedDtoProperties(Type targetDtoType);

        IDtoLoadRule GetLoadRule(Type type);
    }
}