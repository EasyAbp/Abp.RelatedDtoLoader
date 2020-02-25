using Volo.Abp.Domain.Entities;

namespace EasyAbp.Abp.RelatedDtoLoader.Application.Tests
{
    public class Order<TKey> : Entity<TKey>
    {
        public virtual TKey ProductId { get; protected set; }

        protected Order() { }

        public Order(TKey id, TKey productId)
            : base(id)
        {
            ProductId = productId;
        }
    }
}