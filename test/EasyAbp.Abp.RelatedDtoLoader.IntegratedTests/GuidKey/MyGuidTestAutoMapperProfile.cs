using AutoMapper;
using EasyAbp.Abp.RelatedDtoLoader.Tests;
using System;

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