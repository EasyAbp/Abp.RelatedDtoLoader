using EasyAbp.Abp.RelatedDtoLoader.TestBase.Domain;

namespace EasyAbp.Abp.RelatedDtoLoader.UnitTests.Loader
{
    public class MyUnitTestDtoLoaderProfile : RelatedDtoLoaderProfile.RelatedDtoLoaderProfile
    {
        public MyUnitTestDtoLoaderProfile()
        {
            UseRepositoryLoader<ProductDto, Product>();

            RegisterTargetDto<OrderDto>();
        }
    }
}