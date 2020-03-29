using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Threading;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests.IntegratedTests
{
    public class MyIntegerTestDataBuilder : ITransientDependency
    {
        private readonly IRepository<IntOrder> _orderRepository;
        private readonly IRepository<IntProduct> _productRepository;
        private readonly MyIntegerTestData _testData;

        public MyIntegerTestDataBuilder(
            MyIntegerTestData testData,
            IRepository<IntProduct> productRepository,
            IRepository<IntOrder> orderRepository
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
            await _productRepository.InsertAsync(new IntProduct(_testData.ProductId, "The First Product"));
            await _orderRepository.InsertAsync(new IntOrder(_testData.OrderId, _testData.ProductId));
        }
    }
}