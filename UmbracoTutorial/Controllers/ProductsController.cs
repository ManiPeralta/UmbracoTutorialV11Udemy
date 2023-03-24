using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoTutorial.Core.UmbracoModels;
using UmbracoTutorial.ViewModels.Umbraco;

namespace UmbracoTutorial.Controllers
{
    public class ProductsController : RenderController
    {
        private readonly IPublishedValueFallback _publishedValueFallback;
        public ProductsController(
            ILogger<RenderController> logger, 
            ICompositeViewEngine compositeViewEngine, 
            IUmbracoContextAccessor umbracoContextAccessor,
            IPublishedValueFallback publishedValueFallback) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _publishedValueFallback = publishedValueFallback;
        }

        [HttpGet]
        public IActionResult Index([FromQuery(Name = "maxprice")] decimal? maxPrice)
        {
            var products =  (Products)CurrentPage;

            var allProducts = products?.Children<Product>();

            if(maxPrice is decimal MaxPrice)
            {
                allProducts = allProducts?.Where(x => x.Price <= MaxPrice);
            }

            var vm = new ProductListingViewModel(CurrentPage, _publishedValueFallback)
            {
                Products = allProducts.ToList()
            };

            return CurrentTemplate(vm);
        }
    }
}
