using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public class RelatedEntityDtoLoaderRule<TDto, TEntity, TKey> : RelatedDtoLoaderRule<TDto, TKey>
        where TDto : class, IEntityDto<TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected readonly IServiceProvider _serviceProvider;

        public RelatedEntityDtoLoaderRule(IServiceProvider serviceProvider)
            : base()
        {
            _serviceProvider = serviceProvider;
            SeTDtoRule();
        }

        private void SeTDtoRule()
        {
            Rule = async ids =>
            {
                using var scope = _serviceProvider.CreateScope();

                var repository = scope.ServiceProvider.GetService<IReadOnlyRepository<TEntity, TKey>>();
                var objectMapper = _serviceProvider.GetService<IObjectMapper>();

                var relatedDtos = new List<TEntity>();

                foreach (var id in ids)
                {
                    relatedDtos.Add(id == null ? null : await repository.GetAsync(id).ConfigureAwait(false));
                }

                return objectMapper.Map<IEnumerable<TEntity>, TDto[]>(relatedDtos);
            };
        }
    }
}