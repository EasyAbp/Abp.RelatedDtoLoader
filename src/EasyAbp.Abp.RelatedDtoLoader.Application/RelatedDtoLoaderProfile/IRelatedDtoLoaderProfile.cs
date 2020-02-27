using System;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public interface IRelatedDtoLoaderProfile
    {
        RelatedDtoPropertyCollection GetTargetDtoProperties(Type type);

        IDtoLoadRule GetRule(Type type);
    }
}