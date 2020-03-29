using AutoMapper;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests.IntegratedTests
{
    public class MyGuidTestAutoMapperProfile : Profile
    {
        public MyGuidTestAutoMapperProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<Order, OrderDto>();
        }
    }
}