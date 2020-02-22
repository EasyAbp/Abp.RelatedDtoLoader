# RelatedDtoLoader

An Abp module to help you automatically load related DTO (like ProductDto in OrderDto) under DDD.

# Getting Started

* Install with [AbpHelper](https://github.com/EasyAbp/AbpHelper.GUI)

    Coming soon.

* Install Manually

    1. Install `EasyAbp.Abp.RelatedDtoLoader.Application` NuGet package to `MyProject.Application` project and add the module to `DependsOn()`.

    1. Install `EasyAbp.Abp.RelatedDtoLoader.Application.Contracts` NuGet package to `MyProject.Application.Contracts` project and add the module to `DependsOn()`.

# Usage

1. Make your `Order` and `Product` aggregate roots look like this.

    ```
        public class Order : AggregateRoot<Guid>
        {
            public virtual Guid ProductId { get; protected set; }
            
            // Without: public virtual Product Product { get; set; }
    
            protected Order() { }
            
            public Order(Guid id, Guid productId) : base(id)
            {
                ProductId = productId;
            }
        }
        
        public class Product : AggregateRoot<Guid>
        {
            public virtual string Name { get; protected set; }
    
            protected Product() { }
            
            public Product(Guid id, string name) : base(id)
            {
                Name = name;
            }
        }
    ```

1. Add `RelatedDto` attribute to `Product` property in `OrderDto`.

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
                CreateRule<ProductDto, Product>();
            }
        }
    ```

1. Try to get OrderDto with ProductDto.

    ```
        public class OrderAppService : ApplicationService, IOrderAppService
        {
            // ...
            
            public async Task<OrderDto> GetAsync(Guid id)
            {
                var order = _orderRepository.GetAsync(id);
    
                var orderDto = ObjectMapper.Map<Order, OrderDto>(order);
                
                return await _relatedDtoLoader.LoadAsync(orderDto);   // orderDto.Product should have been loaded.
            }
        }
    ```

See more: [Custom DTO source examples](doc/CustomDtoSource.md).

# Roadmap

- [x] Custom DTO source
- [ ] Support one-to-many relation
- [ ] Support non Guid keys
- [ ] Support nested DTOs loading
- [ ] Get duplicate DTO from memory
- [ ] An option to enable loading deleted DTO
- [ ] Unit test

Thanks [@wakuflair](https://github.com/wakuflair) and [@itryan](https://github.com/itryan) for their contribution in the first version.