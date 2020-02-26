using System;
using Volo.Abp.Domain.Entities;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests
{
    public class Product : Entity<Guid>
    {
        public virtual string Name { get; protected set; }

        protected Product() { }

        public Product(Guid id, string name)
            : base(id)
        {
            Name = name;
        }
    }
}