using System;
using System.Collections.Generic;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public interface IRelatedDtoLoaderProfile
    {
        IReadOnlyDictionary<Type, IDtoLoadRule> DtoLoaderRules { get; }

        IReadOnlyDictionary<Type, RelatedDtoPropertyCollection> TargetDtoPropertyCollections { get; }
    }
}