using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EasyAbp.Abp.RelatedDtoLoader.Configurations;
using EasyAbp.Abp.RelatedDtoLoader.DtoLoadRule;
using EasyAbp.Abp.RelatedDtoLoader.Exceptions;
using EasyAbp.Abp.RelatedDtoLoader.RelatedDtoProperty;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.Abp.RelatedDtoLoader.RelatedDtoLoader
{
    public class RelatedDtoLoader : IRelatedDtoLoader, ITransientDependency
    {
        private readonly IDtoLoaderConfigurationProvider _configuration;
        private readonly IServiceProvider _serviceProvider;

        public RelatedDtoLoader(IServiceProvider serviceProvider, IDtoLoaderConfigurationProvider profile)
        {
            _serviceProvider = serviceProvider;
            _configuration = profile;
        }

        public async Task<IEnumerable<TTargetDto>> LoadListAsync<TTargetDto, TKeyProvider>(
            IEnumerable<TTargetDto> targetDtos, IEnumerable<TKeyProvider> keyProviders)
            where TTargetDto : class
            where TKeyProvider : class
        {
            var targetDtoType = typeof(TTargetDto);

            var relatedDtoProperties = _configuration.GetRelatedDtoProperties(targetDtoType);

            if (relatedDtoProperties == null)
            {
                throw new UnsupportedTargetTypeException(targetDtoType);
            }

            var keyProviderType = typeof(TKeyProvider);
            var isKeyProviderSameType = targetDtoType == keyProviderType;
            var arrTargetDtos = targetDtos.ToArray();
            var arrKeyProviders = keyProviders.ToArray();

            foreach (var relatedProperty in relatedDtoProperties)
            {
                var dtoType = relatedProperty.DtoType;
                var dtoProperty = dtoType.Property;
                var attribute = relatedProperty.Attribute;

                RelatedValueType idType = null;

                if (isKeyProviderSameType)
                {
                    idType = relatedProperty.IdType;
                }
                else
                {
                    if (dtoType.GenericType != null || dtoType.IsArray)
                    {
                        throw new MissingIdPropertyNameException(targetDtoType.Name, dtoProperty.Name);
                    }

                    var idProp = keyProviderType.GetProperty(attribute.IdPropertyName ?? dtoProperty.Name + "Id",
                        BindingFlags.Public | BindingFlags.Instance);

                    if (idProp != null)
                    {
                        idType = new RelatedValueType(idProp);
                    }
                }

                if (idType == null)
                {
                    continue;
                }

                var loaderRule = _configuration.GetLoadRule(dtoType.ElementType);

                if (loaderRule == null)
                {
                    continue;
                }

                if (dtoType.GenericType != null)
                {
                    await InternalLoadDtoEnumerableAsync(relatedProperty, idType, loaderRule, arrTargetDtos,
                        arrKeyProviders);
                }
                else if (dtoType.IsArray)
                {
                    await InternalLoadDtoArrayAsync(relatedProperty, idType, loaderRule, arrTargetDtos,
                        arrKeyProviders);
                }
                else
                {
                    await InternalLoadDtoAsync(relatedProperty, idType, loaderRule, arrTargetDtos, arrKeyProviders);
                }
            }

            return arrTargetDtos;
        }

        private async Task InternalLoadDtoAsync<TTargetDto, TKeyProvider>(RelatedDtoProperty.RelatedDtoProperty relatedProperty,
            RelatedValueType idType, IDtoLoadRule loaderRule, TTargetDto[] arrTargetDtos,
            TKeyProvider[] arrKeyProviders)
            where TTargetDto : class
            where TKeyProvider : class
        {
            var dtoType = relatedProperty.DtoType;

            var keyProviderWithIds =
                arrKeyProviders.ToDictionary(x => x, keyProvider => idType.Property.GetValue(keyProvider));
            var idsToLoad = keyProviderWithIds.Values.Where(x => x != null).ToArray();

            Dictionary<object, object> dictLoadedDtos = null;

            if (idsToLoad.Any())
            {
                var relatedDtos = (await loaderRule.LoadAsObjectAsync(_serviceProvider, idsToLoad)).ToArray();
                dictLoadedDtos = relatedDtos.ToDictionary(x => loaderRule.GetKey(x), x => x);
            }

            for (var index = 0; index < arrTargetDtos.Length; index++)
            {
                var targetDto = arrTargetDtos[index];
                var keyProvider = arrKeyProviders[index];

                object propValue = null;

                var desiredDtoKey = keyProviderWithIds[keyProvider];

                if (desiredDtoKey != null && dictLoadedDtos.ContainsKey(desiredDtoKey))
                {
                    propValue = dictLoadedDtos[desiredDtoKey];
                }

                dtoType.Property.SetValue(targetDto, propValue);
            }
        }

        private async Task InternalLoadDtoEnumerableAsync<TTargetDto, TKeyProvider>(RelatedDtoProperty.RelatedDtoProperty relatedProperty,
            RelatedValueType idType, IDtoLoadRule loaderRule, TTargetDto[] arrTargetDtos,
            TKeyProvider[] arrKeyProviders)
            where TTargetDto : class
            where TKeyProvider : class
        {
            var dtoType = relatedProperty.DtoType;

            var keyProviderWithIds = arrKeyProviders.ToDictionary(x => x,
                keyProvider => ((IEnumerable) idType.Property.GetValue(keyProvider)).Cast<object>().ToArray());
            
            var idsToLoad = keyProviderWithIds.Values.Where(x => x != null).SelectMany(x => x).Where(x => x != null)
                .Distinct().ToArray();

            Dictionary<object, object> dictLoadedDtos = null;

            var dtoElementType = dtoType.ElementType;

            if (idsToLoad.Any())
            {
                var relatedDtos = (await loaderRule.LoadAsObjectAsync(_serviceProvider, idsToLoad)).ToArray();
                dictLoadedDtos = relatedDtos.ToDictionary(x => loaderRule.GetKey(x), x => x);
            }

            for (var index = 0; index < arrTargetDtos.Length; index++)
            {
                var targetDto = arrTargetDtos[index];
                var keyProvider = arrKeyProviders[index];

                object propValue = null;

                var desiredDtoKeys = keyProviderWithIds[keyProvider];

                if (desiredDtoKeys != null)
                {
                    var dtoList = Activator.CreateInstance(relatedProperty.DtoListType);

                    foreach (var desiredDtoKey in desiredDtoKeys)
                    {
                        object dto = null;

                        if (desiredDtoKey != null && dictLoadedDtos.ContainsKey(desiredDtoKey))
                        {
                            dto = dictLoadedDtos[desiredDtoKey];
                        }

                        relatedProperty.Add.Invoke(dtoList, new[] {dto});
                    }

                    propValue = dtoList;
                }

                dtoType.Property.SetValue(targetDto, propValue);
            }
        }

        private async Task InternalLoadDtoArrayAsync<TTargetDto, TKeyProvider>(RelatedDtoProperty.RelatedDtoProperty relatedProperty,
            RelatedValueType idType, IDtoLoadRule loaderRule, TTargetDto[] arrTargetDtos,
            TKeyProvider[] arrKeyProviders)
            where TTargetDto : class
            where TKeyProvider : class
        {
            var dtoType = relatedProperty.DtoType;

            var keyProviderWithIds = arrKeyProviders.ToDictionary(x => x,
                keyProvider => ((IEnumerable) idType.Property.GetValue(keyProvider)).Cast<object>().ToArray());
            
            var idsToLoad = keyProviderWithIds.Values.Where(x => x != null).SelectMany(x => x).Where(x => x != null)
                .Distinct().ToArray();

            Dictionary<object, object> dictLoadedDtos = null;

            var dtoElementType = dtoType.ElementType;

            if (idsToLoad.Any())
            {
                var relatedDtos = (await loaderRule.LoadAsObjectAsync(_serviceProvider, idsToLoad)).ToArray();
                dictLoadedDtos = relatedDtos.ToDictionary(loaderRule.GetKey, x => x);
            }

            for (var index = 0; index < arrTargetDtos.Length; index++)
            {
                var targetDto = arrTargetDtos[index];
                var keyProvider = arrKeyProviders[index];

                object propValue = null;

                var desiredDtoKeys = keyProviderWithIds[keyProvider];

                if (desiredDtoKeys != null)
                {
                    var dtoArray = Array.CreateInstance(dtoType.ElementType, desiredDtoKeys.Length);

                    for (var i = 0; i < desiredDtoKeys.Length; i++)
                    {
                        var desiredDtoKey = desiredDtoKeys[i];

                        object dto = null;

                        if (desiredDtoKey != null && dictLoadedDtos.ContainsKey(desiredDtoKey))
                        {
                            dto = dictLoadedDtos[desiredDtoKey];
                        }

                        dtoArray.SetValue(dto, i);
                    }

                    propValue = dtoArray;
                }

                dtoType.Property.SetValue(targetDto, propValue);
            }
        }
    }
}