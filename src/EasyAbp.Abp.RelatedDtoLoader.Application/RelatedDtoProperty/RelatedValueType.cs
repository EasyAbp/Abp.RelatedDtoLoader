using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public class RelatedValueType
    {       
        public PropertyInfo Property { get; private set; }
        public Type GenericType { get; private set; }
        public Type ElementType { get; private set; }
        public bool IsArray { get; private set; }

        public RelatedValueType(PropertyInfo property)
        {
            Property = property;
            DetectType();
        }  
        
        private void DetectType()
        {
            var propType = Property.PropertyType;
            var isEnumerable = typeof(IEnumerable).IsAssignableFrom(propType);

            if (propType.HasElementType && propType.IsArray)
            {
                IsArray = true;
                ElementType = propType.GetElementType();
            }
            else if (isEnumerable)
            {
                GenericType = propType.GetGenericTypeDefinition();
                ElementType = propType.GetGenericArguments()[0];
            }
            else
            {
                ElementType = propType;
            }
        }
    }
}