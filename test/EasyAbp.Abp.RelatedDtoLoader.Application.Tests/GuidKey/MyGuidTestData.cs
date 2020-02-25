using System;

namespace EasyAbp.Abp.RelatedDtoLoader.Application.Tests
{
    public class MyGuidTestData : IMyTestData<Guid>
    {
        public Guid ProductId { get; } = Guid.NewGuid();
        public Guid OrderId { get; } = Guid.NewGuid();
    }       
}