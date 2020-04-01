using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public class DtoLoaderConfigurationExpression 
    {
        private static readonly Type EntityDtoType = typeof(IEntityDto);
        private static readonly Type ProfileType = typeof(IRelatedDtoLoaderProfile);

        private readonly IList<IRelatedDtoLoaderProfile> _profiles = new List<IRelatedDtoLoaderProfile>();

        public DtoLoaderConfigurationExpression()
        {
        }

        public bool AutoUseRepositoryLoader { get; set; }

        public IEnumerable<IRelatedDtoLoaderProfile> Profiles => _profiles;

        public void AddProfile(IRelatedDtoLoaderProfile profile)
        {
            _profiles.Add(profile);
        }

        public void AddAssemblies(RelatedDtoLoaderAssemblyOptions options, params Assembly[] assembliesToScan)
            => AddAssembliesCore(options, assembliesToScan);

        private void AddAssembliesCore(RelatedDtoLoaderAssemblyOptions options, IEnumerable<Assembly> assembliesToScan)
        {
            var allTypes = assembliesToScan.Where(a => !a.IsDynamic && a != typeof(NamedProfile).Assembly).SelectMany(a => a.DefinedTypes)
                .Where(x => !x.IsAbstract)
                .ToArray();

            var profileTypes = allTypes.Where(x => ProfileType.IsAssignableFrom(x)).ToArray();

            foreach (var type in profileTypes)
            {
                var profile = (IRelatedDtoLoaderProfile)Activator.CreateInstance(type);
                AddProfile(profile);
            }

            var dynamicLoaderProfile = new NamedProfile();

            if (options.AutoEnableTargetDtoTypes)
            {
                var dtoTypes = allTypes.Where(x => EntityDtoType.IsAssignableFrom(x)).ToArray();

                foreach (var type in dtoTypes)
                {
                    if (EntityDtoType.IsAssignableFrom(type))
                        dynamicLoaderProfile.EnableTargetDto(type);
                }
            }

            AddProfile(dynamicLoaderProfile);
        }

        private class NamedProfile : RelatedDtoLoaderProfile
        {
           
        }
    }
}
