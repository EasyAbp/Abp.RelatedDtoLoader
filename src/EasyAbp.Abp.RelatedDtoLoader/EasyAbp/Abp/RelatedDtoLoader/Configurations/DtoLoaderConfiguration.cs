using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using EasyAbp.Abp.RelatedDtoLoader.DtoLoadRule;
using EasyAbp.Abp.RelatedDtoLoader.RelatedDtoLoaderProfile;
using EasyAbp.Abp.RelatedDtoLoader.RelatedDtoProperty;

namespace EasyAbp.Abp.RelatedDtoLoader.Configurations
{
    public class DtoLoaderConfiguration : IDtoLoaderConfigurationProvider
    {
        private readonly Dictionary<Type, IDtoLoadRule> _dtoLoaderRules = new Dictionary<Type, IDtoLoadRule>();

        private readonly IEnumerable<IRelatedDtoLoaderProfile> _profiles;

        private readonly ConcurrentDictionary<Type, RelatedDtoPropertyCollection> _targetDtoPropertyCollections =
            new ConcurrentDictionary<Type, RelatedDtoPropertyCollection>();

        public DtoLoaderConfiguration(DtoLoaderConfigurationExpression configurationExpression)
        {
            _profiles = configurationExpression.Profiles;

            FinalizeProfiles();
        }

        public DtoLoaderConfiguration(Action<DtoLoaderConfigurationExpression> configure)
            : this(Build(configure))
        {
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
    }
}