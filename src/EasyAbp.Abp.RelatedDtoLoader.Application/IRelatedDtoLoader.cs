using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public interface IRelatedDtoLoader
    {
        Task<TEntityDto> LoadAsync<TEntityDto>(TEntityDto entityDto)
            where TEntityDto : class, IEntityDto;

        Task<TEntityDto[]> LoadAsync<TEntityDto>(TEntityDto[] entityDtos)
            where TEntityDto : class, IEntityDto;

        Task<TEntityDto> LoadAsync<TEntityDto, TIdFromType>(TEntityDto entityDto, TIdFromType idFromObject)
            where TEntityDto : class, IEntityDto
            where TIdFromType : class;

        Task<TEntityDto[]> LoadAsync<TEntityDto, TIdFromType>(TEntityDto[] entityDtos, TIdFromType[] idFromObjects) 
            where TEntityDto : class, IEntityDto 
            where TIdFromType : class;
    }
}