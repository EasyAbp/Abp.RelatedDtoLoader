using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<TTargetDto>> LoadListAsync<TTargetDto, TKeyProvider>(IEnumerable<TTargetDto> targetDtos, IEnumerable<TKeyProvider> keyProviders)
            where TTargetDto : class
            where TKeyProvider : class
        {
            var targetDtoType = typeof(TTargetDto);

            var keyProviderType = typeof(TKeyProvider);

            var isKeyProviderSameType = targetDtoType == keyProviderType;

            var arrTargetDtos = targetDtos.ToArray();
            var arrKeyProviders = keyProviders.ToArray();

            var relatedDtoProperties = _profile.GetTargetDtoProperties(targetDtoType);

            foreach (var relatedDtoProperty in relatedDtoProperties)
            {
                var dtoProperty = relatedDtoProperty.DtoProperty;
                var attribute = relatedDtoProperty.Attribute;

                var idProperty = isKeyProviderSameType
                    ? relatedDtoProperty.DtoIdProperty
                    : keyProviderType.GetProperty(attribute.IdPropertyName ?? dtoProperty.Name + "Id", BindingFlags.Public | BindingFlags.Instance);

                if (idProperty == null)
                {
                    continue;
                }

                var loaderRule = _profile.GetRule(dtoProperty.PropertyType);

                if (loaderRule == null)
                {
                    continue;
                }

                var keyProviderWithIds = arrKeyProviders.ToDictionary(x => x, dto => idProperty.GetValue(dto));
                var idsToLoad = keyProviderWithIds.Values.Where(x => x != null).ToArray();

                Dictionary<object, object> dictLoadedDtos = null;

                if (idsToLoad.Any())
                {
                    object[] relatedDtos = (await loaderRule.LoadAsObjectAsync(idsToLoad).ConfigureAwait(false)).ToArray();
                    dictLoadedDtos = relatedDtos.ToDictionary(x => loaderRule.GetKey(x), x => x);
                }

                for (var index = 0; index < arrTargetDtos.Length; index++)
                {
                    var targetDto = arrTargetDtos[index];
                    var keyProvider = arrKeyProviders[index];

                    object propValue = null;

                    var desiredDtoKey = keyProviderWithIds[keyProvider];

                    if (desiredDtoKey != null)
                    {                                               
                        propValue = GetDesiredDto(desiredDtoKey, dictLoadedDtos);                        
                    }

                    dtoProperty.SetValue(targetDto, propValue);
                }
            }

            return arrTargetDtos;
        }

        private TDto GetDesiredDto<TDto>(object desiredKey, Dictionary<object, TDto> availableDtos)
            where TDto : class
        {
            if (availableDtos.TryGetValue(desiredKey, out var dto))
            {
                return dto;
            }

            return null;
        }
    }
}