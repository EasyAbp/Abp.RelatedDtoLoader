using AutoMapper;
using EasyAbp.Abp.RelatedDtoLoader.TestBase.Domain;

namespace EasyAbp.Abp.RelatedDtoLoader.IntegratedTests.GuidKey
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