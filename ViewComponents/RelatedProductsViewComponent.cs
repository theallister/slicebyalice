using Microsoft.AspNetCore.Mvc;
using SliceByAlice.Controllers;
using SliceByAlice.Services;
using ContentModels = Umbraco.Cms.Web.Common.PublishedModels;

namespace SliceByAlice.ViewComponents
{
    public class RelatedProductsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public RelatedProductsViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public IViewComponentResult Invoke(int productId, int take = 4)
        {
            var relatedProducts = _productService.GetRelatedProducts(productId, take);
            return View(relatedProducts);
        }
    }
}