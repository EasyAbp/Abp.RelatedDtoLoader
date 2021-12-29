using System.Linq;
using System.Threading.Tasks;
using EasyAbp.Abp.RelatedDtoLoader.RelatedDtoLoader;
using EasyAbp.Abp.RelatedDtoLoader.TestBase.Domain.IntegerKey;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace EasyAbp.Abp.RelatedDtoLoader.IntegratedTests.IntegerKey.Tests
{
    public class RelatedDtoLoader_Basic_Test_Integer : RelatedDtoLoaderTestBase<MyIntegerRelatedDtoLoaderTestModule>
    {
        public RelatedDtoLoader_Basic_Test_Integer()
        {
            TestData = GetRequiredService<MyIntegerTestData>();

            _productRepository = GetRequiredService<IRepository<IntProduct>>();
            _orderRepository = GetRequiredService<IRepository<IntOrder>>();
        }

        protected MyIntegerTestData TestData { get; }
        protected IRepository<IntProduct> _productRepository;
        protected IRepository<IntOrder> _orderRepository;

        [Fact]
        public async Task Should_Load_Related_ProductDto()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var order = await _orderRepository.FirstOrDefaultAsync();

                var orderDto = ObjectMapper.Map<IntOrder, IntOrderDto>(order);

                await _relatedDtoLoader.LoadAsync(orderDto);

                orderDto.Product.ShouldNotBeNull();
            });
        }
    }
}