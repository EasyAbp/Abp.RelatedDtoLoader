using Volo.Abp.Application.Dtos;

namespace EasyAbp.Abp.RelatedDtoLoader.TestBase.Domain.IntegerKey
{
    public class IntProductDto : EntityDto<int>
    {
        public string Name { get; set; }
    }
}