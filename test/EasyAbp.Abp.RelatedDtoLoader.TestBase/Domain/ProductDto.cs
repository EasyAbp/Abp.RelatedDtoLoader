using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.Abp.RelatedDtoLoader.Tests
{
    public class ProductDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}