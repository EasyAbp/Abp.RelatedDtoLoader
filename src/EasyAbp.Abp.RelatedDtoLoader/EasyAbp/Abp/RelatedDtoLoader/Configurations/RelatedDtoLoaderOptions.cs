using System;
using System.Collections.Generic;
using EasyAbp.Abp.RelatedDtoLoader.RelatedDtoLoaderProfile;

namespace EasyAbp.Abp.RelatedDtoLoader.Configurations
{
    public class RelatedDtoLoaderOptions
    {
        public RelatedDtoLoaderOptions()
        {
            Configurators = new List<Action<IRelatedDtoLoaderConfigurationContext>>();
        }

        public List<Action<IRelatedDtoLoaderConfigurationContext>> Configurators { get; }

        public RelatedDtoLoaderAssemblyOptions RegisterTargetDtosInModule<TModule>()
        {
            var assembly = typeof(TModule).Assembly;

            Configurators.Add(context => { context.ConfigurationExpression.AddAssemblies(null, assembly); });

            return null;
        }

        public void AddProfile<TProfile>()
            where TProfile : IRelatedDtoLoaderProfile, new()
        {
            Configurators.Add(context => { context.ConfigurationExpression.AddProfile(new TProfile()); });
        }
    }
}