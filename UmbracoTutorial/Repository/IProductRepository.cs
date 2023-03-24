using UmbracoTutorial.Core.Models;
using UmbracoTutorial.Core.UmbracoModels;

namespace UmbracoTutorial.Repository
{
    public interface IProductRepository
    {
        List<Product> GetProducts(string? productSKU, decimal? maxPrice);

        Product Get(int  id);

        Product Create(ProductCreationItem product);

        Product Update(int id, ProductUpdateItem product);

        bool Delete(int Id);
    }
}
