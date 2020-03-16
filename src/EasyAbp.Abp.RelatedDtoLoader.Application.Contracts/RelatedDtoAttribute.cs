using System;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RelatedDtoAttribute : Attribute
    {
        public string IdPropertyName = null;

        public RelatedDtoAttribute() { }

        public RelatedDtoAttribute(string idPropertyName)
        {
            IdPropertyName = idPropertyName;
        }
    }
}