using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public interface IRelatedDtoLoaderRule<TDto, TKey> : IRelatedDtoLoaderRule
        where TDto : class, IEntityDto
    {
        Task<TDto> LoadDtoAsync(TKey id);
    }  
    
    public interface IRelatedDtoLoaderRule 
    {
        Task<IEnumerable<object>> LoadDtoObjectsAsync(IEnumerable<object> ids);
    }
}