using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoTutorial.Core.Models;
using UmbracoTutorial.Core.UmbracoModels;
using UmbracoTutorial.Repository;
using UmbracoTutorial.ViewModels.Api;

namespace UmbracoTutorial.Controllers
{
    // /umbraco/api/productapi/{action}

    [Route("api/products")]
    public class ProductApiController : UmbracoApiController
    {
        private readonly IProductRepository _productRepository;
        private readonly IUmbracoMapper _mapper;
        public ProductApiController(IProductRepository productRepository, IUmbracoMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public record ProductReadRequest(string? productSKU, decimal? maxPrice);

        [HttpGet]
        public IActionResult Read([FromQuery] ProductReadRequest request)
        {
            var mapped = _mapper.MapEnumerable<Product, ProductApiResponseItem>(_productRepository.GetProducts(request.productSKU, request.maxPrice));

            return Ok(mapped);
        }


        [HttpPost]
        public IActionResult Create([FromBody]ProductCreationItem request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Fields errors");
            }

            var product = _productRepository.Create(request);

            if(product == null)
            {
                StatusCode(StatusCodes.Status500InternalServerError, $"Error creating product");
            }

            return Ok(_mapper.Map<Product, ProductApiResponseItem>(product));
        }


        [HttpPut]
        public IActionResult Update()
        {
            return Ok("Update");
        }


        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var result =_productRepository.Delete(id);

            return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting product id: {id}");

        }
    }
}
