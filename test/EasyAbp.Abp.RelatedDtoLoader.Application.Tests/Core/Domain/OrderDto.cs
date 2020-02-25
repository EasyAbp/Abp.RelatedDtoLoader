using Volo.Abp.Application.Dtos;

namespace EasyAbp.Abp.RelatedDtoLoader.Application.Tests
{
    public class OrderDto<TKey> : EntityDto<TKey>
    {
        public TKey ProductId { get; set; }
        
        [RelatedDto]
        public ProductDto<TKey> Product { get; set; }
    }
}