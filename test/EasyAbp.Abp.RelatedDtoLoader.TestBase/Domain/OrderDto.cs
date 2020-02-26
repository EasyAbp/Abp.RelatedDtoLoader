using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests
{
    public class OrderDto : EntityDto<Guid>
    {
        public Guid ProductId { get; set; }

        [RelatedDto]
        public ProductDto Product { get; set; }

        public OrderDto Clone()
        {
            return (OrderDto)this.MemberwiseClone();
        }
    }
}