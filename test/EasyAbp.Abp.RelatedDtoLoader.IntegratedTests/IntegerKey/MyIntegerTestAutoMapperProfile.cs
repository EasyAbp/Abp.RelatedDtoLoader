using AutoMapper;
using EasyAbp.Abp.RelatedDtoLoader.Tests;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests.IntegratedTests
{
    public class MyIntegerTestAutoMapperProfile : Profile
    {
        public MyIntegerTestAutoMapperProfile()
        {
            CreateMap<IntProduct, IntProductDto>();
            CreateMap<IntOrder, IntOrderDto>();
        }
    }
}