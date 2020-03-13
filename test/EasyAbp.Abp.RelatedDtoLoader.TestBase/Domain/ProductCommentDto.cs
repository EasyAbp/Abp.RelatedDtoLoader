using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests
{
    public class ProductCommentDto : EntityDto<Guid>
    {
        public Guid Id { get; set; }

        public string Content { get; set; }
    }
}