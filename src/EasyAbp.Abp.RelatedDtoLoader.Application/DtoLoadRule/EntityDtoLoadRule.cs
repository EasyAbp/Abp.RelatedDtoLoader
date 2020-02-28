using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public class EntityDtoLoadRule<TDto, TEntity, TKey> : DtoLoadRule<TDto, TKey>
        where TDto : class, IEntityDto<TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected readonly IServiceProvider _serviceProvider;

        public EntityDtoLoadRule(IServiceProvider serviceProvider)
            : base()
        {
            _serviceProvider = serviceProvider;
            SetDtoRule();
        }

        private void SetDtoRule()
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