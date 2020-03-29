using AutoMapper;
using EasyAbp.Abp.RelatedDtoLoader.TestBase.Domain.IntegerKey;

namespace EasyAbp.Abp.RelatedDtoLoader.IntegratedTests.IntegerKey
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