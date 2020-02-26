using System;
using Volo.Abp.Domain.Entities;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests
{
    public class IntOrder : Entity<int>
    {
        public virtual int ProductId { get; protected set; }

        protected IntOrder() { }

        public IntOrder(int id, int productId)
            : base(id)
        {
            ProductId = productId;
        }
    }
}