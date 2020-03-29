namespace EasyAbp.Abp.RelatedDtoLoader.Tests.IntegratedTests
{
    public class MyGuidRelatedDtoLoaderProfile : RelatedDtoLoaderProfile
    {
        public MyGuidRelatedDtoLoaderProfile()
        {
            UseRepositoryLoader<ProductDto, Product>();
        }
    }
}