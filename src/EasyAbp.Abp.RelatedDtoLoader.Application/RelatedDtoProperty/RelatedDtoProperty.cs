using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public class RelatedDtoProperty
    {       
        public RelatedDtoAttribute Attribute { get; private set; }

        public RelatedValueType DtoType { get; private set; }
        public RelatedValueType IdType { get; private set; }

        public Type DtoListType { get; private set; }
        public MethodInfo Add { get; private set; }

        public RelatedDtoProperty(RelatedDtoAttribute attribute, RelatedValueType dtoType, RelatedValueType idType)
        {
            Attribute = attribute;
            DtoType = dtoType;
            IdType = idType;

            BuildDtoListType();
        }   
        
        private void BuildDtoListType()
        {
            var dtoListType  = typeof(List<>).MakeGenericType(DtoType.ElementType);
            DtoListType = dtoListType;
            Add = dtoListType.GetMethod(nameof(IList.Add), BindingFlags.Public | BindingFlags.Instance);
        }
    }
}