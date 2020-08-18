# Example 1

Get DTOs from application service

```
    public class MyProjectRelatedDtoLoaderProfile : RelatedDtoLoaderProfile
    {
        public MyProjectRelatedDtoLoaderProfile(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            CreateRule<ProductDto>(async ids =>
            {
                var dtos = new List<ProductDto>();
                
                using (var scope = serviceProvider.CreateScope())
                {
                    var productAppService = scope.ServiceProvider.GetService<IProductAppService>();
                    
                    foreach (var id in ids)
                    {
                        dtos.Add(await productAppService.GetAsync(id));
                    }
                }

                return dtos;
            });
        }
    }
```