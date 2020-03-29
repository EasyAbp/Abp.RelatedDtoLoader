using AutoMapper;

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