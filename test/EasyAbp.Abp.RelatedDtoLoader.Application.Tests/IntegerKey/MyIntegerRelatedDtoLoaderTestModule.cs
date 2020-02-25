using Microsoft.Extensions.DependencyInjection;
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
        typeof(MyIntegerEntityFrameworkCoreTestModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpRelatedDtoLoaderApplicationModule)
        )]
    public class MyIntegerRelatedDtoLoaderTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<MyIntegerTestAutoMapperProfile>();
            });

            context.Services
                .AddSingleton<IRelatedDtoLoaderProfile, MyIntegerRelatedDtoLoaderProfile>()
                .AddSingleton<IMyTestData<int>, MyIntegerTestData>()
                .AddSingleton<MyTestDataBuilder<int>>()
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
                    .GetRequiredService<MyTestDataBuilder<int>>()
                    .BuildAsync());
            }
        }
    }
}