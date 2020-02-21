using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public class RelatedDtoLoaderRule<TEntityDto> : IRelatedDtoLoaderRule<TEntityDto> where TEntityDto : class, IEntityDto
    {
        private readonly IServiceProvider _serviceProvider;
        
        private Func<IEnumerable<Guid?>, Task<TEntityDto[]>> Rule { get; set; }
        
        public RelatedDtoLoaderRule(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void SetEntitySource<TEntity>() where TEntity : class, IEntity<Guid>
        {
            Rule = async ids =>
            {
                using var scope = _serviceProvider.CreateScope();
                
                var repository = scope.ServiceProvider.GetService<IReadOnlyRepository<TEntity, Guid>>();
                var objectMapper = _serviceProvider.GetService<IObjectMapper>();
                
                var relatedDtos = new List<TEntity>();
                
                foreach (var id in ids)
                {
                    relatedDtos.Add(id.HasValue ? await repository.GetAsync(id.Value).ConfigureAwait(false) : null);
                }
                    
                return objectMapper.Map<IEnumerable<TEntity>, TEntityDto[]>(relatedDtos);
            };
        }
        
        public void SetCustomSource(Func<IEnumerable<Guid?>, Task<TEntityDto[]>> rule)
        {
            Rule = rule;
        }

        public async Task<TEntityDto> LoadDtoAsync(Guid id)
        {
            return (await LoadDtosAsync(new Guid?[] {id}).ConfigureAwait(false)).First();
        }
        
        public async Task<TEntityDto[]> LoadDtosAsync(IEnumerable<Guid?> ids)
        {
            return await Rule(ids).ConfigureAwait(false);
        }

        public async Task<object[]> LoadDtoObjectsAsync(IEnumerable<Guid?> ids)
        {
            return (await Rule(ids).ConfigureAwait(false)).AsEnumerable<object>().ToArray();
        }
    }
}