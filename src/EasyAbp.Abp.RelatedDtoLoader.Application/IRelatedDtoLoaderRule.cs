using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public interface IRelatedDtoLoaderRule<TEntityDto> : IRelatedDtoLoaderRule
        where TEntityDto : class, IEntityDto
    {
        Task<TEntityDto> LoadDtoAsync(Guid id);

        Task<TEntityDto[]> LoadDtosAsync(IEnumerable<Guid?> ids);

        void SetEntitySource<TEntity>() where TEntity : class, IEntity<Guid>;
        
        void SetCustomSource(Func<IEnumerable<Guid?>, Task<TEntityDto[]>> rule);
    }  
    
    public interface IRelatedDtoLoaderRule 
    {
        Task<object[]> LoadDtoObjectsAsync(IEnumerable<Guid?> ids);
    }
}