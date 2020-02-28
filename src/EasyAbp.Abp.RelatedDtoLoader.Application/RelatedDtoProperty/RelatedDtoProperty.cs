using System;
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
        public PropertyInfo DtoProperty { get; private set; }
        public PropertyInfo DtoIdProperty { get; private set; }

        public RelatedDtoProperty(RelatedDtoAttribute attribute, PropertyInfo property, PropertyInfo dtoIdProperty)
        {
            Attribute = attribute;
            DtoProperty = property;
            DtoIdProperty = dtoIdProperty;
        }        
    }
}