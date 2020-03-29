using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests.IntegratedTests
{
    public class RelatedDtoLoader_Basic_Test_Guid : RelatedDtoLoaderTestBase<MyGuidRelatedDtoLoaderTestModule>
    {
        public RelatedDtoLoader_Basic_Test_Guid()
        {
            TestData = GetRequiredService<MyGuidTestData>();

            _productRepository = GetRequiredService<IRepository<Product>>();
            _orderRepository = GetRequiredService<IRepository<Order>>();
        }

        protected MyGuidTestData TestData { get; }
        protected IRepository<Product> _productRepository;
        protected IRepository<Order> _orderRepository;

        [Fact]
        public async Task Should_Load_Related_ProductDto()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var order = _orderRepository.FirstOrDefault();

                var orderDto = ObjectMapper.Map<Order, OrderDto>(order);

                await _relatedDtoLoader.LoadAsync(orderDto);

                orderDto.Product.ShouldNotBeNull();
            });
        }
    }
}