using EasyAbp.Abp.RelatedDtoLoader.Tests;
using System;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests.IntegratedTests
{
    public class MyIntegerRelatedDtoLoaderProfile : RelatedDtoLoaderProfile
    {
        public MyIntegerRelatedDtoLoaderProfile(IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
            CreateRule<IntProductDto, IntProduct, int>();
        }
    }
}