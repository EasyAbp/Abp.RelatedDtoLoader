﻿using System;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public class UnsupportedTargetTypeException : Exception
    {
        public UnsupportedTargetTypeException(Type type)
            : base(GetMessage(type))
        {
        }

        private static string GetMessage(Type type)
        {
            return $"Unsupported target dto type {type.FullName}.";
        }
    }
}