using EasyAbp.Abp.RelatedDtoLoader.Tests;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Threading;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests.IntegratedTests
{
    public class MyGuidTestDataBuilder : ITransientDependency
    {
        private readonly MyGuidTestData _testData;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Order> _orderRepository;

        public MyGuidTestDataBuilder(
            MyGuidTestData testData,
            IRepository<Product> productRepository,
            IRepository<Order> orderRepository
            )
        {
            _testData = testData;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public void Build()
        {
            AsyncHelper.RunSync(BuildAsync);
        }

        public async Task BuildAsync()
        {
            await _productRepository.InsertAsync(new Product(_testData.ProductId, "The First Product"));
            await _orderRepository.InsertAsync(new Order(_testData.OrderId, _testData.ProductId));
        }
    }
}
