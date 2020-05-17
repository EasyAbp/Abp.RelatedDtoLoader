# RelatedDtoLoader

An Abp module that help you automatically load related DTO (like ProductDto in OrderDto) under DDD.

## Getting Started

* Install with [AbpHelper](https://github.com/EasyAbp/AbpHelper.GUI)

    Coming soon.

* Install Manually

    1. Install `EasyAbp.Abp.RelatedDtoLoader.Abstractions` NuGet package to `MyProject.Application.Contracts` project and add `[DependsOn(AbpRelatedDtoLoaderAbstractionsModule)]` attribute to the module.
    
    1. Install `EasyAbp.Abp.RelatedDtoLoader` NuGet package to `MyProject.Application` project (or any other project you want) and add `[DependsOn(AbpRelatedDtoLoaderModule)]` attribute to the module.

## Usage

1. Make your `Order` **entity** (or aggregate root) like this.

    ```csharp
        public class Order : AggregateRoot<Guid>
        {
            public virtual Guid ProductId { get; protected set; }
            
            // do not add navigation properties to other aggregate roots!
            // public virtual Product Product { get; set; }
    
            protected Order() { }
            
            public Order(Guid id, Guid productId) : base(id)
            {
                ProductId = productId;
            }
        }
    ```

1. Add `RelatedDto` attribute in `OrderDto`.

    ```csharp
        public class OrderDto : EntityDto<Guid>
        {
            public Guid ProductId { get; set; }
            
            [RelatedDto]
            public ProductDto Product { get; set; }
        }
    ```

1. Create `MyProjectRelatedDtoLoaderProfile` and add a rule.

    ```csharp
        public class MyProjectRelatedDtoLoaderProfile : RelatedDtoLoaderProfile
        {
            public MyRelatedDtoLoaderProfile()
            {                
                // the following example gets entities from a repository and maps them to DTOs.
                UseRepositoryLoader<ProductDto, Product>();
                
                // or load it by a customized function.
                UseLoader(GetOrderDtosAsync);

                // a target type need to be enabled to load its related Dtos properties.
                // LoadForDto<OrderDto>();
            }
        }
    ```

1. Configure the RelatedDtoLoader to use the profile

    ```csharp
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // ...

            Configure<RelatedDtoLoaderOptions>(options =>
            {
                // add the Profile
                options.AddProfile<MyProjectRelatedDtoLoaderProfile>();
            });

            // ...
        }
    ```

1. Enable the target type to load its related Dto properties

    either in the Profile

    ```csharp
        public class MyProjectRelatedDtoLoaderProfile : RelatedDtoLoaderProfile
        {
            public MyRelatedDtoLoaderProfile()
            {             
                // ...
                   
                // a target type need to be enabled to load its related Dtos properties.
                LoadForDto<OrderDto>();
            }
        }
    ```

    or via `RegisterTargetDtosInModule method` of RelatedDtoLoaderOptions

    ```csharp
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // ...

            Configure<RelatedDtoLoaderOptions>(options =>
            {                                
                // adding module will auto register all the target dto types which contain any property with RelatedDto attribute.
                options.LoadForDtosInModule<MyApplicationContractsModule>();
            });

            // ...
        }
    ```

1. Try to get OrderDto with ProductDto.

    ```csharp
        public class OrderAppService : ApplicationService, IOrderAppService
        {
            private readonly IRelatedDtoLoader _relatedDtoLoader;
            private readonly IRepository<Order, Guid> _orderRepository;
            
            public OrderAppService(IRelatedDtoLoader relatedDtoLoader, IRepository<Order, Guid> orderRepository)
            {
                _relatedDtoLoader = relatedDtoLoader;
                _orderRepository = orderRepository;
            }
            
            public async Task<OrderDto> GetAsync(Guid id)
            {
                var order = _orderRepository.GetAsync(id);
    
                var orderDto = ObjectMapper.Map<Order, OrderDto>(order);
                
                return await _relatedDtoLoader.LoadAsync(orderDto);   // orderDto.Product should have been loaded.
            }
        }
    ```

See more: [Custom DTO source examples](doc/CustomDtoSource.md).

## Roadmap

- [x] Custom DTO source
- [x] Support one-to-many relation
- [x] Support non Guid keys
- [x] Support multi module development
- [ ] Support nested DTOs loading
- [ ] Get duplicate DTO from memory
- [ ] DTO cache
- [ ] An option to enable loading deleted DTO
- [x] Unit test

Thanks [@wakuflair](https://github.com/wakuflair) and [@itryan](https://github.com/itryan) for their contribution in the first version.
