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
        private Dictionary<Type, IRelatedDtoLoaderRule> Rules { get; }

        public RelatedDtoLoaderProfile(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            
            Rules = new Dictionary<Type, IRelatedDtoLoaderRule>();
        }

        public IRelatedDtoLoaderRule<TDto, Guid> CreateRule<TDto, TEntity>()
            where TDto : class, IEntityDto
            where TEntity : class, IEntity<Guid>
        {
            return CreateRule<TDto, TEntity, Guid>();
        }

        public IRelatedDtoLoaderRule<TDto, TKey> CreateRule<TDto, TEntity, TKey>() 
            where TDto : class, IEntityDto 
            where TEntity : class, IEntity<TKey>
        {
            var rule = new RelatedEntityDtoLoaderRule<TDto, TEntity, TKey>(_serviceProvider);
            
            Rules[typeof(TDto)] = rule;

            return rule;
        }
        
        public IRelatedDtoLoaderRule<TDto, Guid> CreateRule<TDto>(Func<IEnumerable<Guid>, Task<IEnumerable<TDto>>> source)
            where TDto : class, IEntityDto
        {
            return CreateRule<TDto, Guid>(source);
        }

        public IRelatedDtoLoaderRule<TDto, TKey> CreateRule<TDto, TKey>(Func<IEnumerable<TKey>, Task<IEnumerable<TDto>>> source)
            where TDto : class, IEntityDto
        {
            var rule = new RelatedDtoLoaderRule<TDto, TKey>(source);

            Rules[typeof(TDto)] = rule;

            return rule;
        }

        public IRelatedDtoLoaderRule GetRule(Type type)
        {
            return !Rules.TryGetValue(type, out var rule) ? null : rule;
        }
    }
}