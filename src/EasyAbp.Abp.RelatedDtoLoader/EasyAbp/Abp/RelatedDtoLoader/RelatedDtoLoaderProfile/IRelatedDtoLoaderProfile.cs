using System;
using System.Collections.Generic;
using EasyAbp.Abp.RelatedDtoLoader.DtoLoadRule;
using EasyAbp.Abp.RelatedDtoLoader.RelatedDtoProperty;

namespace EasyAbp.Abp.RelatedDtoLoader.RelatedDtoLoaderProfile
{
    public interface IRelatedDtoLoaderProfile
    {
        IReadOnlyDictionary<Type, IDtoLoadRule> DtoLoaderRules { get; }

        IReadOnlyDictionary<Type, RelatedDtoPropertyCollection> TargetDtoPropertyCollections { get; }
    }
}