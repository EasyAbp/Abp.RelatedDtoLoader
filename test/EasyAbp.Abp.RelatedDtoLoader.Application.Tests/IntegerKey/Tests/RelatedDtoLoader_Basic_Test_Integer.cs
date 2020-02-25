using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EasyAbp.Abp.RelatedDtoLoader.Application.Tests
{
    public class RelatedDtoLoader_Basic_Test_Integer : RelatedDtoLoaderTestBase<MyIntegerRelatedDtoLoaderTestModule, int>
    {
        [Fact]
        public async Task Should_Load_Related_ProductDto()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var order = _orderRepository.FirstOrDefault();

                OrderDto<int> orderDto = ObjectMapper.Map<Order<int>, OrderDto<int>>(order);

                await _relatedDtoLoader.LoadAsync(orderDto);

                orderDto.Product.ShouldNotBeNull();
            });
        }
    }
}