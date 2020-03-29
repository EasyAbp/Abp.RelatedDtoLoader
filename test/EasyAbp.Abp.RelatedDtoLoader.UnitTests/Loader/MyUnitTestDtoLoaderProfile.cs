using EasyAbp.Abp.RelatedDtoLoader.Tests;

namespace EasyAbp.Abp.RelatedDtoLoader.UnitTests
{
    public class MyUnitTestDtoLoaderProfile : RelatedDtoLoaderProfile
    {
        public MyUnitTestDtoLoaderProfile()
        {
            UseRepositoryLoader<ProductDto, Product>();

            EnableTargetDto<OrderDto>();
        }
    }
}