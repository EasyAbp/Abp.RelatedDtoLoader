using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.Abp.RelatedDtoLoader.DtoLoadRule;
using EasyAbp.Abp.RelatedDtoLoader.RelatedDtoProperty;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace EasyAbp.Abp.RelatedDtoLoader.RelatedDtoLoaderProfile
{
    public abstract class RelatedDtoLoaderProfile : IRelatedDtoLoaderProfile
    {
        private readonly Dictionary<Type, IDtoLoadRule> _dtoLoaderRules = new Dictionary<Type, IDtoLoadRule>();

        private readonly Dictionary<Type, RelatedDtoPropertyCollection> _targetDtoPropertyCollection =
            new Dictionary<Type, RelatedDtoPropertyCollection>();

        public IReadOnlyDictionary<Type, IDtoLoadRule> DtoLoaderRules => _dtoLoaderRules;

        public IReadOnlyDictionary<Type, RelatedDtoPropertyCollection> TargetDtoPropertyCollections =>
            _targetDtoPropertyCollection;

        public IDtoLoadRule UseRepositoryLoader<TDto, TEntity>()
            where TDto : class, IEntityDto<Guid>
            where TEntity : class, IEntity<Guid>
        {
            return UseRepositoryLoader<TDto, TEntity, Guid>();
        }

        public IDtoLoadRule UseRepositoryLoader<TDto, TEntity, TKey>()
            where TDto : class, IEntityDto<TKey>
            where TEntity : class, IEntity<TKey>
        {
            var source = BuildRepositoryLoader<TDto, TEntity, TKey>();
            var rule = UseLoader(source);

            return rule;
        }

        public static Func<IServiceProvider, IEnumerable<TKey>, Task<IEnumerable<TDto>>> BuildRepositoryLoader<TDto,
            TEntity, TKey>()
            where TEntity : class, IEntity<TKey>
            where TDto : class, IEntityDto<TKey>
        {
            Func<IServiceProvider, IEnumerable<TKey>, Task<IEnumerable<TDto>>> source = async (serviceProvider, ids) =>
            {
                var repository = serviceProvider.GetService<IReadOnlyRepository<TEntity, TKey>>();
                var objectMapper = serviceProvider.GetService<IObjectMapper>();

                var relatedDtos = new List<TEntity>();

                foreach (var id in ids)
                {
                    relatedDtos.Add(id == null ? null : await repository.GetAsync(id));
                }

                return objectMapper.Map<IEnumerable<TEntity>, TDto[]>(relatedDtos);
            };

            return source;
        }

        public IDtoLoadRule UseLoader<TDto>(Func<IServiceProvider, IEnumerable<Guid>, Task<IEnumerable<TDto>>> source)
            where TDto : class, IEntityDto<Guid>
        {
            return UseLoader<TDto, Guid>(source);
        }

        public IDtoLoadRule UseLoader<TDto, TKey>(
            Func<IServiceProvider, IEnumerable<TKey>, Task<IEnumerable<TDto>>> source)
            where TDto : class, IEntityDto<TKey>
        {
            var rule = new DtoLoadRule<TDto, TKey>(source);

            _dtoLoaderRules[typeof(TDto)] = rule;

            return rule;
        }

        public void EnableTargetDto<TTargetDto>()
        {
            EnableTargetDto(typeof(TTargetDto));
        }

        public void EnableTargetDto(Type targetDtoType)
        {
            var props = new RelatedDtoPropertyCollection(targetDtoType);

            if (props.Any())
            {
                _targetDtoPropertyCollection[targetDtoType] = props;
            }
        }
    }
}