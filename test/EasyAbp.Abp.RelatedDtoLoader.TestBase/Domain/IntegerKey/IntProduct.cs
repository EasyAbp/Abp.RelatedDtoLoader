using Volo.Abp.Domain.Entities;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests
{
    public class IntProduct : Entity<int>
    {
        protected IntProduct()
        {
        }

        public IntProduct(int id, string name)
            : base(id)
        {
            Name = name;
        }

        public virtual string Name { get; protected set; }
    }
}