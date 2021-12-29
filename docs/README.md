# Abp.RelatedDtoLoader

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2FEasyAbp%2FAbp.RelatedDtoLoader%2Fmaster%2FDirectory.Build.props)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/EasyAbp.Abp.RelatedDtoLoader.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.Abp.RelatedDtoLoader)
[![NuGet Download](https://img.shields.io/nuget/dt/EasyAbp.Abp.RelatedDtoLoader.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.Abp.RelatedDtoLoader)
[![Discord online](https://badgen.net/discord/online-members/S6QaezrCRq?label=Discord)](https://discord.gg/S6QaezrCRq)
[![GitHub stars](https://img.shields.io/github/stars/EasyAbp/Abp.RelatedDtoLoader?style=social)](https://www.github.com/EasyAbp/Abp.RelatedDtoLoader)

An Abp module that help you automatically load related DTO (like ProductDto in OrderDto) under DDD.

## Installation

1. Install the following NuGet packages. ([see how](https://github.com/EasyAbp/EasyAbpGuide/blob/master/docs/How-To.md#add-nuget-packages))

    * EasyAbp.Abp.RelatedDtoLoader
    * EasyAbp.Abp.RelatedDtoLoader.Abstractions

1. Add `DependsOn(typeof(AbpRelatedDtoLoaderModule))` attribute to configure the module dependencies. ([see how](https://github.com/EasyAbp/EasyAbpGuide/blob/master/docs/How-To.md#add-module-dependencies))

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

1. Configure the RelatedDtoLoader to use the profile.
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

1. Enable the target type to load its related Dto properties.

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

See more: [Custom DTO source examples](/docs/CustomDtoSource.md).

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
