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
        private ConcurrentDictionary<Type, IDtoLoadRule> RelatedDtoRules { get; }

        private readonly ConcurrentDictionary<Type, RelatedDtoPropertyCollection> _targetDtoPropertyCollection = new ConcurrentDictionary<Type, RelatedDtoPropertyCollection>();

        private static readonly Type EntityDtoType = typeof(IEntityDto);

        public RelatedDtoLoaderProfile()
        {
            RelatedDtoRules = new ConcurrentDictionary<Type, IDtoLoadRule>();
        }

        protected void AddModule<TType>()
        {
            var dtoTypes = typeof(TType).Assembly.GetTypes().Where(x => EntityDtoType.IsAssignableFrom(x)).ToArray();

            foreach (var dtoType in dtoTypes)
            {
                TryRegisterTargetDto(dtoType);
            }
        }

        protected void RegisterTargetDto<TTargetDto>()
            where TTargetDto : class
        {
            TryRegisterTargetDto(typeof(TTargetDto));
        }

        private void TryRegisterTargetDto(Type targetDtoType)
        {
            if (_targetDtoPropertyCollection.ContainsKey(targetDtoType))
                return;

            var props = new RelatedDtoPropertyCollection(targetDtoType);

            if (props.Any())
            {
                _targetDtoPropertyCollection.TryAdd(targetDtoType, props);
            }
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
            var rule = new EntityDtoLoadRule<TDto, TEntity, TKey>();

            RelatedDtoRules[typeof(TDto)] = rule;

            return rule;
        }

        protected IDtoLoadRule CreateRule<TDto>(Func<IServiceProvider, IEnumerable<Guid>, Task<IEnumerable<TDto>>> source)
            where TDto : class, IEntityDto<Guid>
        {
            return CreateRule<TDto, Guid>(source);
        }

        protected IDtoLoadRule CreateRule<TDto, TKey>(Func<IServiceProvider, IEnumerable<TKey>, Task<IEnumerable<TDto>>> source)
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
            return _targetDtoPropertyCollection.ContainsKey(targetDtoType)
                ? _targetDtoPropertyCollection[targetDtoType] 
                : null;
        }
    }
}