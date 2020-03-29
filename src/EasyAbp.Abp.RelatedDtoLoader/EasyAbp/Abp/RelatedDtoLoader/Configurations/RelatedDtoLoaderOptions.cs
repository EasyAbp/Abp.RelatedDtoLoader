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

        public RelatedDtoLoaderAssemblyOptions AddModule<TModule>(RelatedDtoLoaderAssemblyOptions options = null)
        {
            var assembly = typeof(TModule).Assembly;

            options ??= new RelatedDtoLoaderAssemblyOptions();

            Configurators.Add(context => { context.ConfigurationExpression.AddAssemblies(options, assembly); });

            return options;
        }

        public void AddProfile<TProfile>()
            where TProfile : IRelatedDtoLoaderProfile, new()
        {
            Configurators.Add(context => { context.ConfigurationExpression.AddProfile(new TProfile()); });
        }
    }
}