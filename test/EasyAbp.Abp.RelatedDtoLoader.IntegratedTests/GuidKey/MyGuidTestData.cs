using System;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests.IntegratedTests
{
    public class MyGuidTestData : ISingletonDependency
    {
        public Guid ProductId { get; } = Guid.NewGuid();
        public Guid OrderId { get; } = Guid.NewGuid();
    }
}