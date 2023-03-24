using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoTutorial.Core.Services;
using UmbracoTutorial.ViewModels;

namespace UmbracoTutorial.Controllers
{
    public class ProductController : UmbracoPageController, IVirtualPageController
    {
        private readonly IProductService _productService;
        private readonly IUmbracoContextAccessor _contextAccessor;
        public ProductController(ILogger<UmbracoPageController> logger, 
            ICompositeViewEngine compositeViewEngine,
            IProductService productService,
            IUmbracoContextAccessor umbracoContextAccessor) : base(logger, compositeViewEngine)
        {
            _productService = productService;
            _contextAccessor = umbracoContextAccessor;
        }

        public IPublishedContent? FindContent(ActionExecutingContext actionExecutingContext)
        {
            var homepage = _contextAccessor.GetRequiredUmbracoContext()
                ?.Content?.GetAtRoot().FirstOrDefault();

            var productListingpage = homepage?.FirstChildOfType("products");
            return productListingpage ?? homepage;
        }

        public IActionResult Details(int id)
        {
            // ProductDTO
            var product = _productService.GetAll().FirstOrDefault(x=>x.Id == id);

            //if ProductDTO return  not found
            if(product == null || CurrentPage == null)
            {
                return NotFound();
            }

            //vm
            var vm = new ProductViewModel(CurrentPage)
            {
                ProductName = product.Name
            };
            // return View(vm)

            return View("Product/Details", vm);
        }
    }
}
