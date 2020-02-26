using EasyAbp.Abp.RelatedDtoLoader.Tests;
using Microsoft.EntityFrameworkCore;
using System;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests.IntegratedTests
{
    public class MyGuidDbContext : AbpDbContext<MyGuidDbContext>, IEfCoreDbContext
    {
        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

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
