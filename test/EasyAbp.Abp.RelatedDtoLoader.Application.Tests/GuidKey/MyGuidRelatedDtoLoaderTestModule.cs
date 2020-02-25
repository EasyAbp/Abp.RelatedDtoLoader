using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace EasyAbp.Abp.RelatedDtoLoader.Application.Tests
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
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<MyGuidTestAutoMapperProfile>();
            });

            context.Services
                .AddSingleton<IRelatedDtoLoaderProfile, MyGuidRelatedDtoLoaderProfile>()
                .AddSingleton<IMyTestData<Guid>, MyGuidTestData>()
                .AddSingleton<MyTestDataBuilder<Guid>>()
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
                    .GetRequiredService<MyTestDataBuilder<Guid>>()
                    .BuildAsync());
            }
        }
    }
}