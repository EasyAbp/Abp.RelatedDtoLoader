using System;
using EasyAbp.Abp.RelatedDtoLoader.DtoLoadRule;
using EasyAbp.Abp.RelatedDtoLoader.RelatedDtoProperty;

namespace EasyAbp.Abp.RelatedDtoLoader.Configurations
{
    public interface IDtoLoaderConfigurationProvider
    {
        RelatedDtoPropertyCollection GetRelatedDtoProperties(Type targetDtoType);

        IDtoLoadRule GetLoadRule(Type type);
    }
}