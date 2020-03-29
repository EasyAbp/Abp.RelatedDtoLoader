using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests.IntegratedTests
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