using System;
using Volo.Abp.Domain.Entities;

namespace EasyAbp.Abp.RelatedDtoLoader.TestBase.Domain
{
    public class Order : Entity<Guid>
    {
        protected Order()
        {
        }

        public Order(Guid id, Guid productId)
            : base(id)
        {
            ProductId = productId;
        }

        public virtual Guid ProductId { get; protected set; }
    }
}