using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Testing;
using Volo.Abp.Uow;

namespace EasyAbp.Abp.RelatedDtoLoader.Application.Tests
{
    public abstract class RelatedDtoLoaderTestBase<TTestModule, TKey> : AbpIntegratedTest<TTestModule>
        where TTestModule: AbpModule
    {
        protected IObjectMapper ObjectMapper { get; }
        protected IMyTestData<TKey> TestData { get; }
        protected IRepository<Product<TKey>> _productRepository;
        protected IRepository<Order<TKey>> _orderRepository;
        protected IRelatedDtoLoader _relatedDtoLoader;

        protected RelatedDtoLoaderTestBase()
        {
            ObjectMapper = GetRequiredService<IObjectMapper>();
            TestData = GetRequiredService<IMyTestData<TKey>>();

            _productRepository = GetRequiredService<IRepository<Product<TKey>>>();
            _orderRepository = GetRequiredService<IRepository<Order<TKey>>>();
            _relatedDtoLoader = GetRequiredService<IRelatedDtoLoader>();
        }

        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        protected virtual Task WithUnitOfWorkAsync(Func<Task> func)
        {
            return WithUnitOfWorkAsync(new AbpUnitOfWorkOptions(), func);
        }

        protected virtual async Task WithUnitOfWorkAsync(AbpUnitOfWorkOptions options, Func<Task> action)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

                using (var uow = uowManager.Begin(options))
                {
                    await action();

                    await uow.CompleteAsync();
                }
            }
        }

        protected virtual Task<TResult> WithUnitOfWorkAsync<TResult>(Func<Task<TResult>> func)
        {
            return WithUnitOfWorkAsync(new AbpUnitOfWorkOptions(), func);
        }

        protected virtual async Task<TResult> WithUnitOfWorkAsync<TResult>(AbpUnitOfWorkOptions options, Func<Task<TResult>> func)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

                using (var uow = uowManager.Begin(options))
                {
                    var result = await func();
                    await uow.CompleteAsync();
                    return result;
                }
            }
        }
    }
}