using Umbraco.Cms.Core.Mapping;
using UmbracoTutorial.Core.UmbracoModels;
using UmbracoTutorial.ViewModels.Api;

namespace UmbracoTutorial.Mapping
{
    public class ProductMapping : IMapDefinition
    {
        public void DefineMaps(IUmbracoMapper mapper)
        {
            mapper.Define<Product, ProductApiResponseItem>((source, context) => new ProductApiResponseItem(), Map);
        }

        private void Map(Product source, ProductApiResponseItem target, MapperContext mapperContext)
        {
            target.Id = source.Id;
            target.ProductName = source.ProductName ?? source.Name;
            target.Price = source.Price;
            target.ImageUrl = source.Photos?.Url() ?? "#";
            target.ProductSKU = source.Sku ?? string.Empty;
            target.Categories = source?.Category?.ToList() ?? new List<string>();
            target.Description = source.Description ?? string.Empty;
        }
    }
}
