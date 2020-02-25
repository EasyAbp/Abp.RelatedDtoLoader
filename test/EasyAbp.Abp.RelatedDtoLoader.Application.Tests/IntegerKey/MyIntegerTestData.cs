using Volo.Abp.DependencyInjection;

namespace EasyAbp.Abp.RelatedDtoLoader.Application.Tests
{
    public class MyIntegerTestData : IMyTestData<int>, ISingletonDependency
    {
        public int ProductId { get; } = 123;
        public int OrderId { get; } = 456;
    }       
}