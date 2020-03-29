using EasyAbp.Abp.RelatedDtoLoader.Configurations;
using EasyAbp.Abp.RelatedDtoLoader.IntegratedTests.IntegerKey;
using EasyAbp.Abp.RelatedDtoLoader.TestBase;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace EasyAbp.Abp.RelatedDtoLoader.IntegratedTests.GuidKey
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpTestBaseModule),
        typeof(MyGuidEntityFrameworkCoreTestModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpRelatedDtoLoaderApplicationModule)
    )]
    public class MyGuidRelatedDtoLoaderTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options => { options.AddProfile<MyGuidTestAutoMapperProfile>(); });

            Configure<RelatedDtoLoaderOptions>(options =>
            {
                options.AddProfile<MyGuidRelatedDtoLoaderProfile>();
                options.AddModule<RelatedDtoLoaderTestBaseModule>();
            });

            context.Services
                .AddSingleton<MyGuidTestData>()
                .AddSingleton<MyIntegerTestDataBuilder>()
                ;
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            SeedTestData(context);
        }

        private static void SeedTestData(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                AsyncHelper.RunSync(() => scope.ServiceProvider
                    .GetRequiredService<MyGuidTestDataBuilder>()
                    .BuildAsync());
            }
        }
    }
}