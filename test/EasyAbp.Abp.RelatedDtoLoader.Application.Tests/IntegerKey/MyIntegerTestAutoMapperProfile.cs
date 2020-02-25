using AutoMapper;

namespace EasyAbp.Abp.RelatedDtoLoader.Application.Tests
{
    public class MyIntegerTestAutoMapperProfile : Profile
    {
        public MyIntegerTestAutoMapperProfile()
        {
            CreateMap<Product<int>, ProductDto<int>>();
            CreateMap<Order<int>, OrderDto<int>>();
        }
    }
}