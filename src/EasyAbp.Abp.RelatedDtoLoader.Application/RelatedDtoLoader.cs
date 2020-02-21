using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public class RelatedDtoLoader : IRelatedDtoLoader, ITransientDependency
    {
        private readonly IRelatedDtoLoaderProfile _profile;

        public RelatedDtoLoader(IRelatedDtoLoaderProfile profile)
        {
            _profile = profile;
        }

        public async Task<TEntityDto> LoadAsync<TEntityDto>(TEntityDto entityDto) where TEntityDto : class, IEntityDto
        {
            return (await LoadAsync(new[] {entityDto}).ConfigureAwait(false)).First();
        }

        public async Task<TEntityDto[]> LoadAsync<TEntityDto>(TEntityDto[] entityDtos)
            where TEntityDto : class, IEntityDto
        {
            return await LoadAsync(entityDtos, entityDtos).ConfigureAwait(false);
        }

        public async Task<TEntityDto> LoadAsync<TEntityDto, TIdFromType>(TEntityDto entityDto, TIdFromType idFromObject)
            where TEntityDto : class, IEntityDto where TIdFromType : class
        {
            return (await LoadAsync(new[] {entityDto}, new[] {idFromObject}).ConfigureAwait(false)).First();
        }

        public async Task<TEntityDto[]> LoadAsync<TEntityDto, TIdFromType>(TEntityDto[] entityDtos, TIdFromType[] idFromObjects)
            where TEntityDto : class, IEntityDto
            where TIdFromType : class
        {
            var dtoType = typeof(TEntityDto);

            var idFromType = typeof(TIdFromType);
            
            foreach (var property in dtoType.GetProperties())
            {
                var attribute = property.GetCustomAttribute<RelatedDtoAttribute>(true);
                
                if (attribute == null)
                {
                    continue;
                }

                var idProperty = idFromType.GetProperty(attribute.IdPropertyName ?? property.Name + "Id",
                    BindingFlags.Public | BindingFlags.Instance);
                
                if (idProperty == null)
                {
                    continue;
                }
                
                var loaderRule = _profile.GetRule(property.PropertyType);

                if (loaderRule == null)
                {
                    continue;
                }

                var ids = idFromObjects.Select(dto => (Guid?) idProperty.GetValue(dto));
                
                var relatedDtos = await loaderRule.LoadDtoObjectsAsync(ids).ConfigureAwait(false);
                
                for (var index = 0; index < entityDtos.Length; index++)
                {
                    property.SetValue(entityDtos[index], relatedDtos[index]);
                }
            }

            return entityDtos;
        }
    }
}