using System;

namespace EasyAbp.Abp.RelatedDtoLoader.Application.Tests
{
    public class MyIntegerRelatedDtoLoaderProfile : RelatedDtoLoaderProfile
    {
        public MyIntegerRelatedDtoLoaderProfile(IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
            CreateRule<ProductDto<int>, Product<int>, int>();
        }
    }
}