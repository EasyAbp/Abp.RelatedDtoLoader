using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EasyAbp.Abp.RelatedDtoLoader.Exceptions;

namespace EasyAbp.Abp.RelatedDtoLoader.RelatedDtoProperty
{
    public class RelatedDtoPropertyCollection : IEnumerable<RelatedDtoProperty>
    {
        private readonly Type _targetType;

        private readonly Dictionary<string, RelatedDtoProperty> _rules;

        public RelatedDtoPropertyCollection(Type targetDtoType)
        {
            _targetType = targetDtoType;

            _rules = BuildRules(_targetType);
        }

        public IEnumerator<RelatedDtoProperty> GetEnumerator()
        {
            return _rules.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private Dictionary<string, RelatedDtoProperty> BuildRules(Type targetDtoType)
        {
            var rules = new Dictionary<string, RelatedDtoProperty>();

            var propsForRelatedDto = targetDtoType.GetProperties()
                .Select(x => new
                {
                    Property = x,
                    RelatedDtoAttribute = (RelatedDtoAttribute) x.GetCustomAttribute(typeof(RelatedDtoAttribute), true)
                })
                .Where(x => x.RelatedDtoAttribute != null)
                .ToArray();

            foreach (var propForRelatedDto in propsForRelatedDto)
            {
                var attribute = propForRelatedDto.RelatedDtoAttribute;
                var dtoProperty = propForRelatedDto.Property;
                var dtoPropertyType = dtoProperty.PropertyType;

                var dtoType = new RelatedValueType(dtoProperty);

                var idPropertyName = attribute.IdPropertyName;

                var isList = dtoType.GenericType != null || dtoType.IsArray;

                if (idPropertyName == null)
                {
                    idPropertyName = isList ? $"{dtoPropertyType.Name}Ids" : $"{dtoProperty.Name}Id";
                }

                PropertyInfo idProperty = null;

                if (!string.IsNullOrEmpty(idPropertyName))
                {
                    idProperty = _targetType.GetProperty(idPropertyName, BindingFlags.Public | BindingFlags.Instance);
                }

                if (idProperty == null && isList)
                {
                    throw new MissingIdPropertyNameException(targetDtoType.Name, dtoProperty.Name);
                }

                var idType = idProperty == null ? null : new RelatedValueType(idProperty);

                var rule = new RelatedDtoProperty(attribute, dtoType, idType);
                
                rules.Add(dtoProperty.Name, rule);
            }

            return rules;
        }
    }
}