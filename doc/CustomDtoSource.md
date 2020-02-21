# Example 1

Get DTOs from application service

```
    public class MyProjectRelatedDtoLoaderProfile : RelatedDtoLoaderProfile
    {
        public MyProjectRelatedDtoLoaderProfile(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            CreateRule(async ids =>
            {
                var dtos = new List<ProductDto>();
                
                using (var scope = serviceProvider.CreateScope())
                {
                    var productAppService = scope.ServiceProvider.GetService<IProductAppService>();
                    
                    foreach (var id in ids)
                    {
                        dtos.Add(id.HasValue ? await productAppService.GetAsync(id.Value) : null);
                    }
                }

                return dtos.ToArray();
            });
        }
    }
```