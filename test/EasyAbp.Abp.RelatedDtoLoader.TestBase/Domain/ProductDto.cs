using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests
{
    public class ProductDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public Guid[] CommentIds { get; set; }

        [RelatedDto(nameof(CommentIds))] public IEnumerable<ProductCommentDto> Comments { get; set; }

        [RelatedDto(nameof(CommentIds))] public List<ProductCommentDto> CommentsList { get; set; }

        [RelatedDto(nameof(CommentIds))] public IReadOnlyList<ProductCommentDto> CommentsReadOnlyList { get; set; }

        [RelatedDto(nameof(CommentIds))] public ProductCommentDto[] CommentsArray { get; set; }
    }
}