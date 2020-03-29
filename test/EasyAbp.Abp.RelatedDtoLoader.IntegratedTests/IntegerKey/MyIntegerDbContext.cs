using EasyAbp.Abp.RelatedDtoLoader.TestBase.Domain.IntegerKey;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.Abp.RelatedDtoLoader.IntegratedTests.IntegerKey
{
    public class MyIntegerDbContext : AbpDbContext<MyIntegerDbContext>, IEfCoreDbContext
    {
        public MyIntegerDbContext(DbContextOptions<MyIntegerDbContext> options)
            : base(options)
        {
        }

        public DbSet<IntOrder> Orders { get; set; }

        public DbSet<IntProduct> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}