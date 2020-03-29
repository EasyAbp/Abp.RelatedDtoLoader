using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.Abp.RelatedDtoLoader.TestBase.Domain
{
    public class ProductCommentDto : EntityDto<Guid>
    {
        public Guid Id { get; set; }

        public string Content { get; set; }
    }
}