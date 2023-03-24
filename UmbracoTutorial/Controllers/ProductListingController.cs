using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.BackOffice.Controllers;
using UmbracoTutorial.Core.Services;
using UmbracoTutorial.Core.UmbracoModels;

namespace UmbracoTutorial.Controllers
{
    // /umbraco/backoffice/api/ProductListing/GetProducts?number=1
    public class ProductListingController : UmbracoAuthorizedApiController
    {
        private readonly IProductService _productService;
        public ProductListingController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult GetProducts(int number)
        {
            return Ok(_productService.GetUmbracoProducts(number));
        }
    }
}
