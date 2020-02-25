using AutoMapper;
using System;

namespace EasyAbp.Abp.RelatedDtoLoader.Application.Tests
{
    public class MyGuidTestAutoMapperProfile : Profile
    {
        public MyGuidTestAutoMapperProfile()
        {
            CreateMap<Product<Guid>, ProductDto<Guid>>();
            CreateMap<Order<Guid>, OrderDto<Guid>>();
        }
    }
}