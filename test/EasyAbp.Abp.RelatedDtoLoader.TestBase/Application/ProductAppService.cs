using System;
using EasyAbp.Abp.RelatedDtoLoader.TestBase.Domain;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.Abp.RelatedDtoLoader.TestBase.Application
{
    public class ProductAppService : CrudAppService<Product, ProductDto, Guid>, IProductAppService
    {
        public ProductAppService(IRepository<Product, Guid> repository) : base(repository)
        {
        }
    }
}