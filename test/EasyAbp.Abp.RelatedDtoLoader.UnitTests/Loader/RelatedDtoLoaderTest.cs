using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.Abp.RelatedDtoLoader.Tests;
using Moq;
using Shouldly;
using Xunit;

namespace EasyAbp.Abp.RelatedDtoLoader.UnitTests
{
    public class RelatedDtoLoader_Test
    {
        private readonly MyUnitTestData _testData = new MyUnitTestData();

        private static RelatedDtoLoader GetRelatedDtoLoader(MyUnitTestData testData)
        {
            var fakeConfig = new Mock<IDtoLoaderConfigurationProvider>();

            var fakeProfile = new Mock<IRelatedDtoLoaderProfile>();

            FackProduct(testData, fakeConfig);
            FackProductComment(testData, fakeConfig);

            var orderRelatedDtoProperties = new RelatedDtoPropertyCollection(typeof(OrderDto));
            fakeConfig.Setup(x => x.GetRelatedDtoProperties(typeof(OrderDto)))
                .Returns(orderRelatedDtoProperties);

            var dtoLoader = new RelatedDtoLoader(null, fakeConfig.Object);

            return dtoLoader;
        }

        private static void FackProduct(MyUnitTestData testData, Mock<IDtoLoaderConfigurationProvider> fakeProfile)
        {
            var fakeRule = new Mock<IDtoLoadRule>();

            fakeRule.Setup(x => x.LoadAsObjectAsync(It.IsAny<IServiceProvider>(), It.IsAny<IEnumerable<object>>()))
                .Returns<IServiceProvider, IEnumerable<object>>((serviceProvider, ids) =>
                {
                    return Task.FromResult(testData.ProductDtos.Where(x => ids.Contains(x.Id))
                        .Select(x => (object) x));
                });

            fakeRule.Setup(x => x.GetKey(It.IsAny<object>()))
                .Returns<object>(x => { return ((ProductDto) x).Id; });

            fakeProfile.Setup(x => x.GetLoadRule(typeof(ProductDto)))
                .Returns(fakeRule.Object);

            var productRelatedDtoProperties = new RelatedDtoPropertyCollection(typeof(ProductDto));

            fakeProfile.Setup(x => x.GetRelatedDtoProperties(typeof(ProductDto)))
                .Returns(productRelatedDtoProperties);
        }

        private static void FackProductComment(MyUnitTestData testData,
            Mock<IDtoLoaderConfigurationProvider> fakeProfile)
        {
            var fakeRule = new Mock<IDtoLoadRule>();

            fakeRule.Setup(x => x.LoadAsObjectAsync(It.IsAny<IServiceProvider>(), It.IsAny<IEnumerable<object>>()))
                .Returns<IServiceProvider, IEnumerable<object>>((serviceProvider, ids) =>
                {
                    return Task.FromResult(testData.ProductCommentDtos.Where(x => ids.Contains(x.Id))
                        .Select(x => (object) x));
                });

            fakeRule.Setup(x => x.GetKey(It.IsAny<object>()))
                .Returns<object>(x => { return ((ProductCommentDto) x).Id; });

            fakeProfile.Setup(x => x.GetLoadRule(typeof(ProductCommentDto)))
                .Returns(fakeRule.Object);
        }

        [Fact]
        public async Task Should_Load_Related_OptionalProductDto_for_Single()
        {
            var testData = _testData;

            var dtoLoader = GetRelatedDtoLoader(testData);

            var orders = testData.OrderDtos.ToArray();
            var firstOrder = orders.FirstOrDefault();

            await dtoLoader.LoadAsync(firstOrder);

            firstOrder.OptionalProduct.ShouldNotBeNull();
            firstOrder.Product.Id.ShouldBe(testData.SecondProduct.Id);
        }

        [Fact]
        public async Task Should_Load_Related_ProductCommentDtos()
        {
            var testData = _testData;

            var dtoLoader = GetRelatedDtoLoader(testData);

            var products = testData.ProductDtos.ToArray();
            var secondProduct = products.Skip(1).FirstOrDefault();

            await dtoLoader.LoadAsync(secondProduct);

            secondProduct.Comments.ShouldNotBeNull();
            secondProduct.Comments.Count().ShouldBe(2);

            secondProduct.CommentsList.ShouldNotBeNull();
            secondProduct.CommentsList.Count().ShouldBe(2);

            secondProduct.CommentsReadOnlyList.ShouldNotBeNull();
            secondProduct.CommentsReadOnlyList.Count().ShouldBe(2);

            secondProduct.CommentsArray.ShouldNotBeNull();
            secondProduct.CommentsArray.Count().ShouldBe(2);
        }

        [Fact]
        public async Task Should_Load_Related_ProductDto_for_Single()
        {
            var testData = _testData;

            var dtoLoader = GetRelatedDtoLoader(testData);

            var orders = testData.OrderDtos.ToArray();
            var firstOrder = orders[0];
            var secondOrder = orders[1];

            await dtoLoader.LoadAsync(firstOrder);

            firstOrder.Product.ShouldNotBeNull();
            firstOrder.Product.Id.ShouldBe(testData.SecondProduct.Id);
            firstOrder.OptionalProduct.ShouldNotBeNull();
            firstOrder.OptionalProduct.Id.ShouldBe(testData.SecondProduct.Id);

            secondOrder.OptionalProduct.ShouldBeNull();
        }

        [Fact]
        public async Task Should_Load_Related_ProductDtos_for_Multi()
        {
            var testData = _testData;

            var dtoLoader = GetRelatedDtoLoader(testData);

            var orders = testData.OrderDtos.ToArray();
            var firstOrder = orders[0];
            var secondOrder = orders[1];

            await dtoLoader.LoadListAsync(orders);

            firstOrder.Product.ShouldNotBeNull();
            firstOrder.Product.Id.ShouldBe(testData.SecondProduct.Id);
            firstOrder.OptionalProduct.ShouldNotBeNull();
            firstOrder.OptionalProduct.Id.ShouldBe(testData.SecondProduct.Id);

            secondOrder.OptionalProduct.ShouldBeNull();
        }

        [Fact]
        public async Task Should_Throw_Exception_for_Unsupported_TargetDto()
        {
            var testData = _testData;

            var dtoLoader = GetRelatedDtoLoader(testData);

            var unsupportedOrder = new UnsupportedOrderDto();

            await Should.ThrowAsync<UnsupportedTargetTypeException>(dtoLoader.LoadAsync(unsupportedOrder));
        }
    }
}