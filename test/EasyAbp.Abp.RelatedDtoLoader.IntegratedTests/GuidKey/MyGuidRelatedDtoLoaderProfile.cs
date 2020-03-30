using EasyAbp.Abp.RelatedDtoLoader.TestBase.Application;
using EasyAbp.Abp.RelatedDtoLoader.TestBase.Domain;

namespace EasyAbp.Abp.RelatedDtoLoader.IntegratedTests.GuidKey
{
    public class MyGuidRelatedDtoLoaderProfile : RelatedDtoLoaderProfile.RelatedDtoLoaderProfile
    {
        public MyGuidRelatedDtoLoaderProfile()
        {
            // UseRepositoryLoader<ProductDto, Product>();
            UseAppServiceLoader<ProductDto, IProductAppService>((service, id) => service.GetAsync(id));
        }
    }
}