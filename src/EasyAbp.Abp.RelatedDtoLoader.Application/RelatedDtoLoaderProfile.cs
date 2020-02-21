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

            // Example: CreateRule<ProductDto, Product>();
        }

        public IRelatedDtoLoaderRule<TEntityDto> CreateRule<TEntityDto, TEntity>() 
            where TEntityDto : class, IEntityDto 
            where TEntity : class, IEntity<Guid>
        {
            var rule = new RelatedDtoLoaderRule<TEntityDto>(_serviceProvider);
            
            rule.SetEntitySource<TEntity>();

            Rules[typeof(TEntityDto)] = rule;

            return rule;
        }

        public IRelatedDtoLoaderRule<TEntityDto> CreateRule<TEntityDto>(
            Func<IEnumerable<Guid?>, Task<TEntityDto[]>> source)
            where TEntityDto : class, IEntityDto
        {
            var rule = new RelatedDtoLoaderRule<TEntityDto>(_serviceProvider);

            rule.SetCustomSource(source);

            Rules[typeof(TEntityDto)] = rule;

            return rule;
        }

        public IRelatedDtoLoaderRule GetRule(Type type)
        {
            return !Rules.TryGetValue(type, out var rule) ? null : rule;
        }
    }
}