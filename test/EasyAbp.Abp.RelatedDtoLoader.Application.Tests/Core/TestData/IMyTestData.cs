using Volo.Abp.DependencyInjection;

namespace EasyAbp.Abp.RelatedDtoLoader.Application.Tests
{
    public interface IMyTestData<TKey>
    {
        TKey ProductId { get; }
        TKey OrderId { get; }
    }
}