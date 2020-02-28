using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public abstract class RelatedDtoLoaderProfile : IRelatedDtoLoaderProfile, ISingletonDependency
    {
        private readonly IServiceProvider _serviceProvider;

        private ConcurrentDictionary<Type, IDtoLoadRule> RelatedDtoRules { get; }

        private readonly ConcurrentDictionary<Type, RelatedDtoPropertyCollection> _targetDtoPropertyCollection = new ConcurrentDictionary<Type, RelatedDtoPropertyCollection>();

        protected readonly ConcurrentQueue<Type> _unsupportedTargetDtoTypes = null;

        protected int InvalidTargetDtoTypeCacheCount = 10;

        public RelatedDtoLoaderProfile(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            
            RelatedDtoRules = new ConcurrentDictionary<Type, IDtoLoadRule>();

            _unsupportedTargetDtoTypes = new ConcurrentQueue<Type>();
        }

        protected IDtoLoadRule CreateRule<TDto, TEntity>()
            where TDto : class, IEntityDto<Guid>
            where TEntity : class, IEntity<Guid>
        {
            return CreateRule<TDto, TEntity, Guid>();
        }

        protected IDtoLoadRule CreateRule<TDto, TEntity, TKey>() 
            where TDto : class, IEntityDto<TKey>
            where TEntity : class, IEntity<TKey>
        {
            var rule = new EntityDtoLoadRule<TDto, TEntity, TKey>(_serviceProvider);
            
            RelatedDtoRules[typeof(TDto)] = rule;

            return rule;
        }

        protected IDtoLoadRule CreateRule<TDto>(Func<IEnumerable<Guid>, Task<IEnumerable<TDto>>> source)
            where TDto : class, IEntityDto<Guid>
        {
            return CreateRule<TDto, Guid>(source);
        }

        protected IDtoLoadRule CreateRule<TDto, TKey>(Func<IEnumerable<TKey>, Task<IEnumerable<TDto>>> source)
            where TDto : class, IEntityDto<TKey>
        {
            var rule = new DtoLoadRule<TDto, TKey>(source);

            RelatedDtoRules[typeof(TDto)] = rule;

            return rule;
        }

        public IDtoLoadRule GetRule(Type type)
        {
            return !RelatedDtoRules.TryGetValue(type, out var rule) ? null : rule;
        }

        public RelatedDtoPropertyCollection GetRelatedDtoProperties(Type targetDtoType)
        {
            RelatedDtoPropertyCollection props;

            if (_targetDtoPropertyCollection.TryGetValue(targetDtoType, out props) || _unsupportedTargetDtoTypes.Contains(targetDtoType))
                return props;

            props = CacheTargetDtoType(targetDtoType);

            return props;
        }

        protected RelatedDtoPropertyCollection CacheTargetDtoType(Type targetDtoType)
        {
            var props = new RelatedDtoPropertyCollection(targetDtoType);

            if (props.Any())
            {
                _targetDtoPropertyCollection.TryAdd(targetDtoType, props);
            }
            else
            {
                CacheInvalidTargetDtoType(targetDtoType);
                props = null;
            }

            return props;
        }

        protected void CacheInvalidTargetDtoType(Type targetDtoType)
        {
            if (_unsupportedTargetDtoTypes.Count() >= InvalidTargetDtoTypeCacheCount)
            {
                _unsupportedTargetDtoTypes.TryDequeue(out _);
            }

            _unsupportedTargetDtoTypes.Enqueue(targetDtoType);
        }

    }
}