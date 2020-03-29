namespace EasyAbp.Abp.RelatedDtoLoader.Tests.IntegratedTests
{
    public class MyIntegerRelatedDtoLoaderProfile : RelatedDtoLoaderProfile
    {
        public MyIntegerRelatedDtoLoaderProfile()
        {
            UseRepositoryLoader<IntProductDto, IntProduct, int>();

            EnableTargetDto<IntOrderDto>();
        }
    }
}