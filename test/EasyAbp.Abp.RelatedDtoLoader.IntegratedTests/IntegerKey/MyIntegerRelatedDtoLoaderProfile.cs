using EasyAbp.Abp.RelatedDtoLoader.TestBase.Domain.IntegerKey;

namespace EasyAbp.Abp.RelatedDtoLoader.IntegratedTests.IntegerKey
{
    public class MyIntegerRelatedDtoLoaderProfile : RelatedDtoLoaderProfile.RelatedDtoLoaderProfile
    {
        public MyIntegerRelatedDtoLoaderProfile()
        {
            UseRepositoryLoader<IntProductDto, IntProduct, int>();

            LoadForDto<IntOrderDto>();
        }
    }
}