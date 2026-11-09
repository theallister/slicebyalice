using Microsoft.AspNetCore.Mvc;
using SliceByAlice.Controllers;
using SliceByAlice.Services;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;
using ContentModels = Umbraco.Cms.Web.Common.PublishedModels;

namespace SliceByAlice.ViewComponents
{
    public class ProductSwiperViewComponent : ViewComponent
    {
        private readonly IProductService _productService;
        public ProductSwiperViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ProductBlock productBlock, int take = 4)
        {
            List<Product> products = new List<Product>();

            if (productBlock?.TagCategoryPicker != null && productBlock.TagCategoryPicker.Any())
            {
                foreach (IPublishedContent identifier in productBlock.TagCategoryPicker)
                {
                    List<Product>? relatedProducts = _productService.GetProductsByCategoryOrTag(identifier, take);
                    if (relatedProducts != null && relatedProducts.Any()) products.AddRange(relatedProducts);
                }
            }
            if (productBlock?.ProductPicker != null && productBlock.ProductPicker.Any())
            {
                List<Product?>? pickedProducts = productBlock?.ProductPicker?.Select(p => p as Product)?.Where(p => p != null)?.ToList();
                if (pickedProducts != null && pickedProducts.Any()) products.AddRange(pickedProducts.OfType<Product>());
            }
            return View(products);
        }
    }
}