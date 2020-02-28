using System;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public interface IRelatedDtoLoaderProfile
    {
        RelatedDtoPropertyCollection GetRelatedDtoProperties(Type targetDtoType);

        IDtoLoadRule GetRule(Type type);
    }
}