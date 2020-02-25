using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public class RelatedDtoLoader : IRelatedDtoLoader, ITransientDependency
    {
        private readonly IRelatedDtoLoaderProfile _profile;

        public RelatedDtoLoader(IRelatedDtoLoaderProfile profile)
        {
            _profile = profile;
        }

        public async Task<TDto> LoadAsync<TDto>(TDto targetDto)
            where TDto : class
        {
            return (await LoadListAsync(new[] { targetDto }).ConfigureAwait(false)).First();
        }

        public async Task<IEnumerable<TDto>> LoadListAsync<TDto>(IEnumerable<TDto> targetDtos)
            where TDto : class
        {
            return await LoadListAsync(targetDtos, targetDtos).ConfigureAwait(false);
        }

        public async Task<TDto> LoadAsync<TDto, TKeyProvider>(TDto targetDto, TKeyProvider keyProvider)
            where TDto : class
            where TKeyProvider : class
        {
            return (await LoadListAsync(new[] { targetDto }, new[] { keyProvider }).ConfigureAwait(false)).First();
        }

        public async Task<IEnumerable<TDto>> LoadListAsync<TDto, TKeyProvider>(IEnumerable<TDto> targetDtos, IEnumerable<TKeyProvider> keyProviders)
            where TDto : class
            where TKeyProvider : class
        {
            var dtoType = typeof(TDto);

            var keyProviderType = typeof(TKeyProvider);

            foreach (var property in dtoType.GetProperties())
            {
                var attribute = property.GetCustomAttribute<RelatedDtoAttribute>(true);

                if (attribute == null)
                {
                    continue;
                }

                var idProperty = keyProviderType.GetProperty(attribute.IdPropertyName ?? property.Name + "Id",
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

                var ids = keyProviders.Select(dto => idProperty.GetValue(dto)).ToArray();

                var relatedDtos = (await loaderRule.LoadDtoObjectsAsync(ids).ConfigureAwait(false)).ToArray();

                for (var index = 0; index < targetDtos.Count(); index++)
                {
                    property.SetValue(targetDtos.ElementAt(index), relatedDtos.ElementAt(index));
                }
            }

            return targetDtos;
        }
    }
}