using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public class RelatedDtoLoaderOptions
    {
        public List<Action<IRelatedDtoLoaderConfigurationContext>> Configurators { get; }

        public RelatedDtoLoaderOptions()
        {
            Configurators = new List<Action<IRelatedDtoLoaderConfigurationContext>>();
        }

        public RelatedDtoLoaderAssemblyOptions AddModule<TModule>(RelatedDtoLoaderAssemblyOptions options = null)
        {
            var assembly = typeof(TModule).Assembly;

            options = options ?? new RelatedDtoLoaderAssemblyOptions();

            Configurators.Add(context =>
            {
                context.ConfigurationExpression.AddAssemblies(options, assembly);
            });

            return options;
        }

        public void AddProfile<TProfile>()
            where TProfile : IRelatedDtoLoaderProfile, new()
        {
            Configurators.Add(context =>
            {
                context.ConfigurationExpression.AddProfile(new TProfile());
            });
        }
    }
}
