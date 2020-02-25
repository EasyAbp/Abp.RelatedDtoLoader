using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public interface IRelatedDtoLoader
    {
        Task<TDto> LoadAsync<TDto>(TDto targetDto)
            where TDto : class;

        Task<IEnumerable<TDto>> LoadListAsync<TDto>(IEnumerable<TDto> targetDtos)
            where TDto : class;

        Task<TDto> LoadAsync<TDto, TKeyProvider>(TDto targetDto, TKeyProvider keyProvider)
            where TDto : class
            where TKeyProvider : class;

        Task<IEnumerable<TDto>> LoadListAsync<TDto, TKeyProvider>(IEnumerable<TDto> targetDtos, IEnumerable<TKeyProvider> keyProviders)
            where TDto : class
            where TKeyProvider : class;
    }
}