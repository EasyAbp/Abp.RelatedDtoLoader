using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EasyAbp.Abp.RelatedDtoLoader.Application.Tests
{
    public class RelatedDtoLoader_Basic_Test_Guid : RelatedDtoLoaderTestBase<MyGuidRelatedDtoLoaderTestModule, Guid>
    {
        [Fact]
        public async Task Should_Load_Related_ProductDto()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var order = _orderRepository.FirstOrDefault();

                OrderDto<Guid> orderDto = ObjectMapper.Map<Order<Guid>, OrderDto<Guid>>(order);

                await _relatedDtoLoader.LoadAsync(orderDto);

                orderDto.Product.ShouldNotBeNull();
            });

        }
    }
}