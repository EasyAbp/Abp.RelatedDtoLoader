using System;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RelatedDtoAttribute : Attribute
    {
        public readonly string IdPropertyName;

        public RelatedDtoAttribute()
        {
        }

        public RelatedDtoAttribute(string idPropertyName)
        {
            IdPropertyName = idPropertyName;
        }
    }
}