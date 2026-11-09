using Microsoft.AspNetCore.Mvc;
using SliceByAlice.Services;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace SliceByAlice.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("item/{productId:int}")]
        public IActionResult GetCartItem(int productId)
        {
            var cartItem = _productService.GetCartItem(productId);
            return cartItem is null ? NotFound() : Ok(cartItem);
        }

        [HttpGet("related/{productId:int}")]
        public ActionResult<List<Product>> GetRelatedProducts(int productId, int take = 4)
        {
            return _productService.GetRelatedProducts(productId, take);
        }
    }
}