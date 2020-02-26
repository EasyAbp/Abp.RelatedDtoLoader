using System;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public interface IRelatedDtoLoaderProfile
    {
        IRelatedDtoLoaderRule GetRule(Type type);
    }
}