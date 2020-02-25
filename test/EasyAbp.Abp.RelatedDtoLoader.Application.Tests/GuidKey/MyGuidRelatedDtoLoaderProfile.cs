using System;

namespace EasyAbp.Abp.RelatedDtoLoader.Application.Tests
{
    public class MyGuidRelatedDtoLoaderProfile : RelatedDtoLoaderProfile
    {
        public MyGuidRelatedDtoLoaderProfile(IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
            CreateRule<ProductDto<Guid>, Product<Guid>>();
        }
    }
}