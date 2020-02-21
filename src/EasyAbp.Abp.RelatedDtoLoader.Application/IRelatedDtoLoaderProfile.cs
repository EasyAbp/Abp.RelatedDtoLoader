using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public interface IRelatedDtoLoaderProfile
    {
        IRelatedDtoLoaderRule<TEntityDto> CreateRule<TEntityDto, TEntity>() 
            where TEntityDto : class, IEntityDto
            where TEntity : class, IEntity<Guid>;

        IRelatedDtoLoaderRule<TEntityDto> CreateRule<TEntityDto>(
            Func<IEnumerable<Guid?>, Task<TEntityDto[]>> source)
            where TEntityDto : class, IEntityDto;
        
        IRelatedDtoLoaderRule GetRule(Type type);            
    }
}