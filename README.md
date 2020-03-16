# RelatedDtoLoader

An Abp module to help you automatically load related DTO (like ProductDto in OrderDto) under DDD.

## Getting Started

* Install with [AbpHelper](https://github.com/EasyAbp/AbpHelper.GUI)

    Coming soon.

* Install Manually

    1. Install `EasyAbp.Abp.RelatedDtoLoader.Application` NuGet package to `MyProject.Application` project and add the module to `DependsOn()`.

    1. Install `EasyAbp.Abp.RelatedDtoLoader.Application.Contracts` NuGet package to `MyProject.Application.Contracts` project and add the module to `DependsOn()`.

## Usage

1. Make your `Order` **entity** (or aggregate root) like this.

    ```
        public class Order : AggregateRoot<Guid>
        {
            public virtual Guid ProductId { get; protected set; }
            
            // Do not add navigation properties to other aggregate roots!
            // public virtual Product Product { get; set; }
    
            protected Order() { }
            
            public Order(Guid id, Guid productId) : base(id)
            {
                ProductId = productId;
            }
        }
    ```

1. Add `RelatedDto` attribute in `OrderDto`.

    ```
        public class OrderDto : EntityDto<Guid>
        {
            public Guid ProductId { get; set; }
            
            [RelatedDto]
            public ProductDto Product { get; set; }
        }
    ```

1. Create `MyProjectRelatedDtoLoaderProfile` and add a rule.

    ```
        public class MyProjectRelatedDtoLoaderProfile : RelatedDtoLoaderProfile
        {
            public MyRelatedDtoLoaderProfile(IServiceProvider serviceProvider) : base(serviceProvider)
            {
                // These rules will get entities from generic repository and map them to DTOs.
                
                CreateRule<ProductDto, Product>();  // For Guid (by default) primary key.
                CreateRule<CityDto, City, int>();   // For int primary key.
                
                // Custom DTO source.
                
                CreateRule(GetOrderDtosAsync);
            }
        }
    ```

1. Try to get OrderDto with ProductDto.

    ```
        public class OrderAppService : ApplicationService, IOrderAppService
        {
            private readonly IRelatedDtoLoader _relatedDtoLoader;
            private readonly IRepository<Order, Guid> _orderRepository;
            
            public OrderAppService(
                IRelatedDtoLoader relatedDtoLoader,
                IRepository<Order, Guid> orderRepository)
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
- [ ] Support one-to-many relation
- [x] Support non Guid keys
- [ ] Support multi module development
- [ ] Support nested DTOs loading
- [ ] Get duplicate DTO from memory
- [ ] An option to enable loading deleted DTO
- [x] Unit test

Thanks [@wakuflair](https://github.com/wakuflair) and [@itryan](https://github.com/itryan) for their contribution in the first version.
