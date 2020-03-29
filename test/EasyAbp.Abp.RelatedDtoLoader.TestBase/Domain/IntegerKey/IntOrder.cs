using Volo.Abp.Domain.Entities;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests
{
    public class IntOrder : Entity<int>
    {
        protected IntOrder()
        {
        }

        public IntOrder(int id, int productId)
            : base(id)
        {
            ProductId = productId;
        }

        public virtual int ProductId { get; protected set; }
    }
}