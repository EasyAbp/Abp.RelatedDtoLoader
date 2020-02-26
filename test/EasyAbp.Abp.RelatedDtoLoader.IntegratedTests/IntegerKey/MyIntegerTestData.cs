using Volo.Abp.DependencyInjection;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests.IntegratedTests
{
    public class MyIntegerTestData : ISingletonDependency
    {
        public int ProductId { get; } = 123;
        public int OrderId { get; } = 456;
    }       
}