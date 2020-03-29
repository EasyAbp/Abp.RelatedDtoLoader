using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests
{
    public class UnsupportedOrderDto : EntityDto<Guid>
    {
        public Guid ProductId { get; set; }

        public ProductDto Product { get; set; }

        public Guid? OptionalProductId { get; set; }

        public ProductDto OptionalProduct { get; set; }

        public UnsupportedOrderDto Clone()
        {
            return (UnsupportedOrderDto) MemberwiseClone();
        }
    }
}