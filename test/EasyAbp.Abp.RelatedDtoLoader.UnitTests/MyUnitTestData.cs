using EasyAbp.Abp.RelatedDtoLoader.Tests;
using System;
using System.Linq;

namespace EasyAbp.Abp.RelatedDtoLoader.UnitTests
{
    public class MyUnitTestData
    {
        public readonly ProductDto[] ProductDtos;
        public readonly ProductDto FirstProduct;
        public readonly ProductDto SecondProduct;

        private readonly OrderDto[] _orderDtos;

        public MyUnitTestData()
        {
            var productDto1 = new ProductDto() { Id = Guid.NewGuid(), Name = "Product 1" };
            var productDto2 = new ProductDto() { Id = Guid.NewGuid(), Name = "Product 2" };

            ProductDtos = new ProductDto[] {
               productDto1,
               productDto2,
            };

            FirstProduct = productDto1;
            SecondProduct = productDto2;

            var orderDto1 = new OrderDto() { Id = Guid.NewGuid(), ProductId = productDto2.Id, OptionalProductId = productDto2.Id};
            var orderDto2 = new OrderDto() { Id = Guid.NewGuid(), OptionalProductId = null };

            _orderDtos = new OrderDto[] {
               orderDto1,
               orderDto2,
            };
        }

        public OrderDto[] OrderDtos
        {
            get
            {
                var items = _orderDtos.Select(x => x.Clone()).ToArray();
                return items;
            }
        }
    }
}
