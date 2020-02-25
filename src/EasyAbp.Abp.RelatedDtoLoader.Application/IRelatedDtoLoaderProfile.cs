using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public interface IRelatedDtoLoaderProfile
    {
        IRelatedDtoLoaderRule<TDto, Guid> CreateRule<TDto, TEntity>()
            where TDto : class, IEntityDto
            where TEntity : class, IEntity<Guid>;
        
        IRelatedDtoLoaderRule<TDto, TKey> CreateRule<TDto, TEntity, TKey>() 
            where TDto : class, IEntityDto
            where TEntity : class, IEntity<TKey>;

        IRelatedDtoLoaderRule<TDto, Guid> CreateRule<TDto>(
            Func<IEnumerable<Guid>, Task<IEnumerable<TDto>>> source)
            where TDto : class, IEntityDto;
        
        IRelatedDtoLoaderRule<TDto, TKey> CreateRule<TDto, TKey>(
            Func<IEnumerable<TKey>, Task<IEnumerable<TDto>>> source)
            where TDto : class, IEntityDto;  
        
        IRelatedDtoLoaderRule GetRule(Type type);            
    }
}