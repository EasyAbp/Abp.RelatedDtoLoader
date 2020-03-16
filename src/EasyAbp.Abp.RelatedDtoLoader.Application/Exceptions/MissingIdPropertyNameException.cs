using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public class MissingIdPropertyNameException : Exception
    {
        public MissingIdPropertyNameException(string targetTypeName, string propertyName)
            : base(GetMessage(targetTypeName, propertyName))
        {

        }

        private static string GetMessage(string targetTypeName, string propertyName)
        {
            return $"Missing Id Property Name for {targetTypeName}.{propertyName}.";
        } 
    }
}
