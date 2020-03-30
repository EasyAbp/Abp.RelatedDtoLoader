using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace EasyAbp.Abp.RelatedDtoLoader.TestBase
{
    [DependsOn(typeof(AbpDddApplicationModule))]
    public class RelatedDtoLoaderTestBaseModule : AbpModule
    {
    }
}