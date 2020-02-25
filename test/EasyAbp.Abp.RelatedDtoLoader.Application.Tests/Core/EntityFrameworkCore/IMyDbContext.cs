using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.Abp.RelatedDtoLoader.Application.Tests
{
    public interface IMyDbContext<TKey> : IEfCoreDbContext
    {
        DbSet<Order<TKey>> Orders { get; set; }

        DbSet<Product<TKey>> Products { get; set; }
    }
}