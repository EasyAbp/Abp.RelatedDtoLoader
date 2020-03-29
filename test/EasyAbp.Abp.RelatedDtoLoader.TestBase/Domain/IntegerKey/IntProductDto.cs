using Volo.Abp.Application.Dtos;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests
{
    public class IntProductDto : EntityDto<int>
    {
        public string Name { get; set; }
    }
}