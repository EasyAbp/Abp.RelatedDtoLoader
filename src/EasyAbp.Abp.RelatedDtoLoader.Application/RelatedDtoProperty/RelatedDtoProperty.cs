using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace EasyAbp.Abp.RelatedDtoLoader.RelatedDtoProperty
{
    public class RelatedDtoProperty
    {
        public RelatedDtoProperty(RelatedDtoAttribute attribute, RelatedValueType dtoType, RelatedValueType idType)
        {
            Attribute = attribute;
            DtoType = dtoType;
            IdType = idType;

            BuildDtoListType();
        }

        public RelatedDtoAttribute Attribute { get; }

        public RelatedValueType DtoType { get; }
        
        public RelatedValueType IdType { get; }

        public Type DtoListType { get; private set; }
        
        public MethodInfo Add { get; private set; }

        private void BuildDtoListType()
        {
            var dtoListType = typeof(List<>).MakeGenericType(DtoType.ElementType);
            
            DtoListType = dtoListType;
            
            Add = dtoListType.GetMethod(nameof(IList.Add), BindingFlags.Public | BindingFlags.Instance);
        }
    }
}