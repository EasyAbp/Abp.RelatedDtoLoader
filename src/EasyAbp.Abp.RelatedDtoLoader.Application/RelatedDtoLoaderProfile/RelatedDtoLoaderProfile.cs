using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public abstract class RelatedDtoLoaderProfile : IRelatedDtoLoaderProfile, ISingletonDependency
    {
        private readonly IServiceProvider _serviceProvider;

        private Dictionary<Type, IDtoLoadRule> RelatedDtoRules { get; }

        private Dictionary<Type, RelatedDtoPropertyCollection> _targetDtoPropertyCollection = new Dictionary<Type, RelatedDtoPropertyCollection>();

        public RelatedDtoLoaderProfile(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            
            RelatedDtoRules = new Dictionary<Type, IDtoLoadRule>();
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

        protected RelatedDtoPropertyCollection AddTargetDtoType<TTargetDto>()
            where TTargetDto : class
        {
            return AddTargetDtoType(typeof(TTargetDto));
        }

        protected RelatedDtoPropertyCollection AddTargetDtoType(Type targetDtoType)
        {
            var targetDtoRule = new RelatedDtoPropertyCollection(targetDtoType);

            _targetDtoPropertyCollection.Add(targetDtoType, targetDtoRule);

            return targetDtoRule;
        }        

        public RelatedDtoPropertyCollection GetTargetDtoProperties(Type type)
        {
            return _targetDtoPropertyCollection.ContainsKey(type) ? _targetDtoPropertyCollection[type] : null;
        }
    }
}