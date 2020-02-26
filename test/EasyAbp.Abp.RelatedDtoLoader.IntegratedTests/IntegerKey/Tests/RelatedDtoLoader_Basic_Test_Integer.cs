using EasyAbp.Abp.RelatedDtoLoader.Tests;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests.IntegratedTests
{
    public class RelatedDtoLoader_Basic_Test_Integer : RelatedDtoLoaderTestBase<MyIntegerRelatedDtoLoaderTestModule>
    {
        protected MyIntegerTestData TestData { get; }
        protected IRepository<IntProduct> _productRepository;
        protected IRepository<IntOrder> _orderRepository;

        public RelatedDtoLoader_Basic_Test_Integer()
        {
            TestData = GetRequiredService<MyIntegerTestData>();

            _productRepository = GetRequiredService<IRepository<IntProduct>>();
            _orderRepository = GetRequiredService<IRepository<IntOrder>>();
        }

        [Fact]
        public async Task Should_Load_Related_ProductDto()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var order = _orderRepository.FirstOrDefault();

                IntOrderDto orderDto = ObjectMapper.Map<IntOrder, IntOrderDto>(order);

                await _relatedDtoLoader.LoadAsync(orderDto);

                orderDto.Product.ShouldNotBeNull();
            });
        }
    }
}