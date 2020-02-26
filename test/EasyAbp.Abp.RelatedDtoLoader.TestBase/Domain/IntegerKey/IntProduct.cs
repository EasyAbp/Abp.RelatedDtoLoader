using System;
using Volo.Abp.Domain.Entities;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests
{
    public class IntProduct : Entity<int>
    {
        public virtual string Name { get; protected set; }

        protected IntProduct() { }

        public IntProduct(int id, string name)
            : base(id)
        {
            Name = name;
        }
    }
}