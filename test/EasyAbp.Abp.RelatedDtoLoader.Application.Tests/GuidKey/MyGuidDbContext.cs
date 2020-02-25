using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.Abp.RelatedDtoLoader.Application.Tests
{
    public class MyGuidDbContext : AbpDbContext<MyGuidDbContext>, IMyDbContext<Guid>
    {
        public DbSet<Order<Guid>> Orders { get; set; }

        public DbSet<Product<Guid>> Products { get; set; }

        public MyGuidDbContext(DbContextOptions<MyGuidDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
