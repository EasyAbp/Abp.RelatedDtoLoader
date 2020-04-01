using EasyAbp.Abp.RelatedDtoLoader.Tests;
using System;
using System.Linq;

namespace EasyAbp.Abp.RelatedDtoLoader.UnitTests
{
    public class MyUnitTestDtoLoaderProfile : RelatedDtoLoaderProfile
    {
        public MyUnitTestDtoLoaderProfile()
            : base()
        {
            UseRepositoryLoader<ProductDto, Product>();

            EnableTargetDto<OrderDto>();
        }
    }
}
