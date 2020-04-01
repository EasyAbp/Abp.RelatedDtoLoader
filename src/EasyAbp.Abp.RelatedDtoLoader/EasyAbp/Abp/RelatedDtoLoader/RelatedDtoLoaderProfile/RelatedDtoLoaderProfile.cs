using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.Abp.RelatedDtoLoader.DtoLoadRule;
using EasyAbp.Abp.RelatedDtoLoader.RelatedDtoProperty;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace EasyAbp.Abp.RelatedDtoLoader.RelatedDtoLoaderProfile
{
    public abstract class RelatedDtoLoaderProfile : IRelatedDtoLoaderProfile
    {
        private readonly Dictionary<Type, IDtoLoadRule> _dtoLoaderRules = new Dictionary<Type, IDtoLoadRule>();

        private readonly Dictionary<Type, RelatedDtoPropertyCollection> _targetDtoPropertyCollection =
            new Dictionary<Type, RelatedDtoPropertyCollection>();

        public IReadOnlyDictionary<Type, IDtoLoadRule> DtoLoaderRules => _dtoLoaderRules;

        public IReadOnlyDictionary<Type, RelatedDtoPropertyCollection> TargetDtoPropertyCollections =>
            _targetDtoPropertyCollection;

        public IDtoLoadRule UseRepositoryLoader<TDto, TEntity>()
            where TDto : class, IEntityDto<Guid>
            where TEntity : class, IEntity<Guid>
        {
            return UseRepositoryLoader<TDto, TEntity, Guid>();
        }

        public IDtoLoadRule UseRepositoryLoader<TDto, TEntity, TKey>()
            where TDto : class, IEntityDto<TKey>
            where TEntity : class, IEntity<TKey>
        {
            var source = BuildRepositoryLoader<TDto, TEntity, TKey>();
            
            var rule = UseLoader(source);

            return rule;
        }

        public static Func<IServiceProvider, IEnumerable<TKey>, Task<IEnumerable<TDto>>> BuildRepositoryLoader<TDto,
            TEntity, TKey>()
            where TEntity : class, IEntity<TKey>
            where TDto : class, IEntityDto<TKey>
        {
            return async (serviceProvider, ids) =>
            {
                var repository = serviceProvider.GetService<IReadOnlyRepository<TEntity, TKey>>();
                var objectMapper = serviceProvider.GetService<IObjectMapper>();

                var relatedEntities = new List<TEntity>();

                foreach (var id in ids)
                {
                    relatedEntities.Add(id == null ? null : await repository.GetAsync(id));
                }

                return objectMapper.Map<IEnumerable<TEntity>, TDto[]>(relatedEntities);
            };
        }
        
        public IDtoLoadRule UseAppServiceLoader<TDto, TAppService>(Func<TAppService, Guid, Task<TDto>> itemSource)
            where TDto : class, IEntityDto<Guid>
            where TAppService : IApplicationService
        {
            return UseAppServiceLoader<TDto, TAppService, Guid>(itemSource);
        }
        
        public IDtoLoadRule UseAppServiceLoader<TDto, TAppService, TKey>(Func<TAppService, TKey, Task<TDto>> itemSource)
            where TDto : class, IEntityDto<TKey>
            where TAppService : IApplicationService
        {
            var source = BuildAppServiceLoader(itemSource);

            var rule = UseLoader(source);

            return rule;
        }
        
        public static Func<IServiceProvider, IEnumerable<TKey>, Task<IEnumerable<TDto>>> BuildAppServiceLoader<TDto,
            TAppService, TKey>(Func<TAppService, TKey, Task<TDto>> itemSource)
            where TAppService : IApplicationService
            where TDto : class, IEntityDto<TKey>
        {
            return async (serviceProvider, ids) =>
            {
                var appService = serviceProvider.GetService<TAppService>();

                var relatedDtos = new List<TDto>();

                foreach (var id in ids)
                {
                    relatedDtos.Add(id == null ? null : await itemSource(appService, id));
                }

                return relatedDtos;
            };
        }

        public IDtoLoadRule UseLoader<TDto>(Func<IServiceProvider, IEnumerable<Guid>, Task<IEnumerable<TDto>>> source)
            where TDto : class, IEntityDto<Guid>
        {
            return UseLoader<TDto, Guid>(source);
        }

        public IDtoLoadRule UseLoader<TDto, TKey>(
            Func<IServiceProvider, IEnumerable<TKey>, Task<IEnumerable<TDto>>> source)
            where TDto : class, IEntityDto<TKey>
        {
            var rule = new DtoLoadRule<TDto, TKey>(source);

            _dtoLoaderRules[typeof(TDto)] = rule;

            return rule;
        }

        public void RegisterTargetDto<TTargetDto>()
        {
            RegisterTargetDto(typeof(TTargetDto));
        }

        public void RegisterTargetDto(Type targetDtoType)
        {
            var props = new RelatedDtoPropertyCollection(targetDtoType);

            if (props.Any())
            {
                _targetDtoPropertyCollection[targetDtoType] = props;
            }
        }
    }
}