using System;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RelatedDtoAttribute : Attribute
    {
        public string IdPropertyName = null;
    }
}