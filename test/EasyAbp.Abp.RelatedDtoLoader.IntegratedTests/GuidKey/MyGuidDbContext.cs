using EasyAbp.Abp.RelatedDtoLoader.TestBase.Domain;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.Abp.RelatedDtoLoader.IntegratedTests.GuidKey
{
    public class MyGuidDbContext : AbpDbContext<MyGuidDbContext>, IEfCoreDbContext
    {
        public MyGuidDbContext(DbContextOptions<MyGuidDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}