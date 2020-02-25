using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.Abp.RelatedDtoLoader.Application.Tests
{
    public class MyIntegerDbContext : AbpDbContext<MyIntegerDbContext>, IMyDbContext<int>
    {
        public DbSet<Order<int>> Orders { get; set; }

        public DbSet<Product<int>> Products { get; set; }

        public MyIntegerDbContext(DbContextOptions<MyIntegerDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
