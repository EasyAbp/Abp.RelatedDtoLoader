using EasyAbp.Abp.RelatedDtoLoader.Tests;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EasyAbp.Abp.RelatedDtoLoader.UnitTests
{
    public class RelatedDtoLoader_Test
    {
        private MyUnitTestData _testData = new MyUnitTestData();

        [Fact]
        public async Task Should_Load_Related_ProductDto_for_Single()
        {
            var testData = _testData;

            var dtoLoader = GetRelatedDtoLoader(testData);

            var orders = testData.OrderDtos.ToArray();
            var firstOrder = orders.FirstOrDefault();

            await dtoLoader.LoadAsync(firstOrder);

            firstOrder.Product.ShouldNotBeNull();
            firstOrder.Product.Id.ShouldBe(testData.SecondProduct.Id);
        }

        [Fact]
        public async Task Should_Load_Related_ProductDtos_for_Multi()
        {
            var testData = _testData;

            var dtoLoader = GetRelatedDtoLoader(testData);

            var orders = testData.OrderDtos.ToArray();
            var firstOrder = orders.FirstOrDefault();

            await dtoLoader.LoadListAsync(orders);

            firstOrder.Product.ShouldNotBeNull();
            firstOrder.Product.Id.ShouldBe(testData.SecondProduct.Id);

            var secondOrder = orders[1];
            secondOrder.Product.ShouldBeNull();
        }

        private static RelatedDtoLoader GetRelatedDtoLoader(MyUnitTestData testData)
        {
            var fakeProductRule = new Mock<IRelatedDtoLoaderRule>();

            fakeProductRule.Setup(x => x.LoadAsObjectAsync(It.IsAny<IEnumerable<object>>()))
                .Returns<IEnumerable<object>>(ids => {
                    return Task.FromResult(testData.ProductDtos.Where(x => ids.Contains(x.Id)).Select(x => (object)x));
                });

            fakeProductRule.Setup(x => x.GetKey(It.IsAny<object>()))
                .Returns<object>(x => { return ((ProductDto)x).Id; });

            var fakeProfile = new Mock<IRelatedDtoLoaderProfile>();
            fakeProfile.Setup(x => x.GetRule(typeof(ProductDto)))
                .Returns(fakeProductRule.Object);

            var dtoLoader = new RelatedDtoLoader(fakeProfile.Object);

            return dtoLoader;
        }
    }
}
