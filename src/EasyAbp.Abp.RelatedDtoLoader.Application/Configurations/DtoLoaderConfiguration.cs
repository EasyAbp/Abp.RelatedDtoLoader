using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public class DtoLoaderConfiguration : IDtoLoaderConfigurationProvider
    {
        private Dictionary<Type, IDtoLoadRule> _dtoLoaderRules = new Dictionary<Type, IDtoLoadRule>();

        private readonly ConcurrentDictionary<Type, RelatedDtoPropertyCollection> _targetDtoPropertyCollections = new ConcurrentDictionary<Type, RelatedDtoPropertyCollection>();

        private IEnumerable<IRelatedDtoLoaderProfile> _profiles;

        public DtoLoaderConfiguration(DtoLoaderConfigurationExpression configurationExpression)
        {
            _profiles = configurationExpression.Profiles;

            FinalizeProfiles();
        }

        public DtoLoaderConfiguration(Action<DtoLoaderConfigurationExpression> configure)
            : this(Build(configure))
        {
        }

        private static DtoLoaderConfigurationExpression Build(Action<DtoLoaderConfigurationExpression> configure)
        {
            var expr = new DtoLoaderConfigurationExpression();
            configure(expr);
            return expr;
        }

        private void FinalizeProfiles()
        {
            foreach (var profile in _profiles)
            {
                foreach (var dtoLoaderRule in profile.DtoLoaderRules)
                {
                    _dtoLoaderRules[dtoLoaderRule.Key] = dtoLoaderRule.Value;
                }
                
                foreach (var relatedDtoProps in profile.TargetDtoPropertyCollections)
                {
                    _targetDtoPropertyCollections[relatedDtoProps.Key] = relatedDtoProps.Value;
                }
            }
        }

        public RelatedDtoPropertyCollection GetRelatedDtoProperties(Type targetDtoType)
        {
            return _targetDtoPropertyCollections.ContainsKey(targetDtoType)
                ? _targetDtoPropertyCollections[targetDtoType]
                : null;
        }

        public IDtoLoadRule GetLoadRule(Type type)
        {
            return _dtoLoaderRules.ContainsKey(type) ? _dtoLoaderRules[type] : null;
        }
    }
}
