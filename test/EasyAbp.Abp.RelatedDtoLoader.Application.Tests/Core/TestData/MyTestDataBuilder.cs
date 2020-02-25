using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Threading;

namespace EasyAbp.Abp.RelatedDtoLoader.Application.Tests
{
    public class MyTestDataBuilder<TKey> : ITransientDependency
    {
        private readonly IMyTestData<TKey> _testData;
        private readonly IRepository<Product<TKey>> _productRepository;
        private readonly IRepository<Order<TKey>> _orderRepository;

        public MyTestDataBuilder(
            IMyTestData<TKey> testData,
            IRepository<Product<TKey>> productRepository,
            IRepository<Order<TKey>> orderRepository
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
            await _productRepository.InsertAsync(new Product<TKey>(_testData.ProductId, "The First Product"));
            await _orderRepository.InsertAsync(new Order<TKey>(_testData.OrderId, _testData.ProductId));
        }
    }
}
