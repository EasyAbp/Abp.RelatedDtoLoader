using System;
using System.Linq;
using EasyAbp.Abp.RelatedDtoLoader.TestBase.Domain;

namespace EasyAbp.Abp.RelatedDtoLoader.UnitTests
{
    public class MyUnitTestData
    {
        private readonly OrderDto[] _orderDtos;
        public readonly ProductDto FirstProduct;
        public readonly ProductCommentDto[] ProductCommentDtos;

        public readonly ProductDto[] ProductDtos;
        public readonly ProductDto SecondProduct;

        public MyUnitTestData()
        {
            var commentDto1 = new ProductCommentDto {Id = Guid.NewGuid(), Content = "Comment 1"};
            var commentDto2 = new ProductCommentDto {Id = Guid.NewGuid(), Content = "Comment 1"};

            ProductCommentDtos = new[]
            {
                commentDto1,
                commentDto2
            };

            var productDto1 = new ProductDto {Id = Guid.NewGuid(), Name = "Product 1"};
            var productDto2 = new ProductDto
                {Id = Guid.NewGuid(), Name = "Product 2", CommentIds = new[] {commentDto1.Id, commentDto2.Id}};

            ProductDtos = new[]
            {
                productDto1,
                productDto2
            };

            FirstProduct = productDto1;
            SecondProduct = productDto2;

            var orderDto1 = new OrderDto
                {Id = Guid.NewGuid(), ProductId = productDto2.Id, OptionalProductId = productDto2.Id};
            var orderDto2 = new OrderDto {Id = Guid.NewGuid(), OptionalProductId = null};

            _orderDtos = new[]
            {
                orderDto1,
                orderDto2
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