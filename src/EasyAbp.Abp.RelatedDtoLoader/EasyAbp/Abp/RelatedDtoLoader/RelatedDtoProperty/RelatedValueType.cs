using System;
using System.Collections;
using System.Reflection;

namespace EasyAbp.Abp.RelatedDtoLoader.RelatedDtoProperty
{
    public class RelatedValueType
    {
        public RelatedValueType(PropertyInfo property)
        {
            Property = property;
            DetectType();
        }

        public PropertyInfo Property { get; }
        
        public Type GenericType { get; private set; }
        
        public Type ElementType { get; private set; }
        
        public bool IsArray { get; private set; }

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