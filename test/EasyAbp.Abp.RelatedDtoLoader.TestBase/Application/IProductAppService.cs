using System;
using EasyAbp.Abp.RelatedDtoLoader.TestBase.Domain;
using Volo.Abp.Application.Services;

namespace EasyAbp.Abp.RelatedDtoLoader.TestBase.Application
{
    public interface IProductAppService : ICrudAppService<ProductDto, Guid>
    {
        
    }
}