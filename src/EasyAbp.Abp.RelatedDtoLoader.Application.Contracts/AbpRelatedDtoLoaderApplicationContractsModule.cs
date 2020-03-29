using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    [DependsOn(
        typeof(AbpDddApplicationContractsModule)
    )]
    public class AbpRelatedDtoLoaderApplicationContractsModule : AbpModule
    {
    }
}