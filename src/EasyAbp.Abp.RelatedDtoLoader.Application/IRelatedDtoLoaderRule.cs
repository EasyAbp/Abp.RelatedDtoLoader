using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public interface IRelatedDtoLoaderRule<TDto, TKey> : IRelatedDtoLoaderRule
        where TDto : class, IEntityDto
    {
        Task<IEnumerable<TDto>> LoadAsync(IEnumerable<TKey> ids);
    }  
    
    public interface IRelatedDtoLoaderRule 
    {
        Task<IEnumerable<object>> LoadAsObjectAsync(IEnumerable<object> ids);
    }
}