using EasyAbp.Abp.RelatedDtoLoader.Tests;
using System;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests.IntegratedTests
{
    public class MyIntegerRelatedDtoLoaderProfile : RelatedDtoLoaderProfile
    {
        public MyIntegerRelatedDtoLoaderProfile() 
            : base()
        {
            AddModule<RelatedDtoLoaderTestBaseModule>();

            CreateRule<IntProductDto, IntProduct, int>();
        }
    }
}