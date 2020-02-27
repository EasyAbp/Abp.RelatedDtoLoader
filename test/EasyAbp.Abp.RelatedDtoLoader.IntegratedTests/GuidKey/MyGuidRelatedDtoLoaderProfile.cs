using EasyAbp.Abp.RelatedDtoLoader.Tests;
using System;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests.IntegratedTests
{
    public class MyGuidRelatedDtoLoaderProfile : RelatedDtoLoaderProfile
    {
        public MyGuidRelatedDtoLoaderProfile(IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
            AddTargetDtoType<OrderDto>();

            CreateRule<ProductDto, Product>();
        }
    }
}