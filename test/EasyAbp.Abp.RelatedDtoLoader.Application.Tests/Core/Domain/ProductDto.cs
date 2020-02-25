using Volo.Abp.Application.Dtos;

namespace EasyAbp.Abp.RelatedDtoLoader.Application.Tests
{
    public class ProductDto<TKey> : EntityDto<TKey>
    {
        public string Name { get; set; }
    }
}