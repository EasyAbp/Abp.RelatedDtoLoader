using Volo.Abp.Domain.Entities;

namespace EasyAbp.Abp.RelatedDtoLoader.Application.Tests
{
    public class Product<TKey> : Entity<TKey>
    {
        public virtual string Name { get; protected set; }

        protected Product() { }

        public Product(TKey id, string name)
            : base(id)
        {
            Name = name;
        }
    }
}