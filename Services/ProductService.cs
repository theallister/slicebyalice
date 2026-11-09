using Serilog;
using SliceByAlice.Models;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace SliceByAlice.Services
{
    public interface IProductService
    {
        CartItemDto? GetCartItem(int productId);
        List<Product> GetAllProducts(int ignoreProductId = 0);
        List<Product> GetRelatedProducts(int productId, int take = 4);
        List<Product> GetProductsByCategoryOrTag(IPublishedContent identifier, int take = 4);
        bool IsBlockedByMarket(int productId);
    }
    public class ProductService : IProductService
    {
        private readonly IPublishedContentQuery _publishedContentQuery;

        public ProductService(IPublishedContentQuery publishedContentQuery)
        {
            _publishedContentQuery = publishedContentQuery;
        }
        public CartItemDto? GetCartItem(int productId)
        {
            var product = _publishedContentQuery.Content(productId);
            if (product == null) return null;
            return new CartItemDto
            {
                ProductId = product.Id,
                Quantity = 1,
                ProductType = GetProductTypeAlias(product),
                ImageUrl = product.Value<MediaWithCrops>("image")?.GetCropUrl(width: 120, height: 120, quality: 80),
                Name = product.Name,
                Price = product.Value<int>("price"),
                TwoForPrice = product.Value<int>("buyTwoPrice"),
                Url = product.Url(),
                InStock = product.Value<int>("inStock")
            };
        }
        public string GetProductTypeAlias(IPublishedContent product)
        {
            string? productType = product?.Parent<ProductCategory>()?.CategoryAlias ?? product?.Parent<CategoryPage>()?.CategoryAlias;
            return productType ?? "";
        }
        public List<Product> GetAllProducts(int ignoreProductId = 0)
        {
            var root = _publishedContentQuery.ContentAtRoot().FirstOrDefault();
            if (root == null) return new List<Product>();
            return root.Descendants<Product>().Where(p => p.Id != ignoreProductId).ToList();
        }
        public List<Product> GetRelatedProducts(int productId, int take = 4)
        {
            Product? baseProduct = _publishedContentQuery.Content(productId) as Product;
            var allProducts = GetAllProducts(productId);
            var relatedProducts = new List<Product>();
            if (baseProduct == null) return new List<Product>();
            foreach (var product in allProducts.Where(p => p.Id != productId && !relatedProducts.Contains(p) && IsRelatedProduct(p, baseProduct)))
            {                
                relatedProducts.Add(product);
                if (relatedProducts.Count >= take) break;
            }
            return relatedProducts.Take(take).ToList();
        }
        public bool IsRelatedProduct(Product product, Product baseProduct)
        {
            if (product == null || baseProduct == null) return false;
            bool hasTags = baseProduct?.Tag != null && baseProduct.Tag.Any();
            string productCategory = baseProduct?.Parent<ProductCategory>()?.Name ?? "";
            if (hasTags && product.Tag != null && product.Tag.Any(t => baseProduct?.Tag != null && baseProduct.Tag.Contains(t)))
                return true;
            if (!string.IsNullOrWhiteSpace(productCategory) && product.Parent<ProductCategory>()?.Name == productCategory)
                return true;
            if (baseProduct?.Parent<CategoryPage>() == product.Parent<CategoryPage>())
                return true;
            return false;
        }
        public List<Product> GetRecommendedProducts(int[] productIds, int take = 4)
        {
            var recommendedProducts = new List<Product>();
            foreach (var productId in productIds)
            {
                recommendedProducts.AddRange(GetRelatedProducts(productId, take).Where(p => !recommendedProducts.Contains(p)));
                if (recommendedProducts.Count >= take) break;
            }
            return recommendedProducts.Take(take).ToList();
        }
        public List<Product> GetProductsByCategoryOrTag(IPublishedContent identifier, int take = 4)
        {
            List<Product> products = new List<Product>();
            if (identifier == null) return products;

            var allProducts = GetAllProducts();
            foreach (Product product in allProducts)
            {
                bool matchesCategory = identifier is ProductCategory category && product.Parent<ProductCategory>()?.Id == category.Id ||
                    identifier is CategoryPage categoryPage && product.Parent<CategoryPage>()?.Id == categoryPage.Id;

                bool matchesTag = identifier is ProductTag tag && product?.Tag != null && product.Tag.Any(t => t.Id == tag.Id);
                
                if (product != null && (matchesCategory || matchesTag))
                {
                    products.Add(product);
                    if (products.Count >= take) break;
                }
            }
            return products;
        }
        public bool IsBlockedByMarket(int productId)
        {
            var root = _publishedContentQuery.ContentAtRoot().FirstOrDefault();
            if (root is not StartPage startPage) return false;

            List<Product> blockedProducts = [];
            
            if (startPage.AtMarket && startPage.BlockedProducts != null && startPage.BlockedProducts.Any())
            {
                foreach (var identifier in startPage.BlockedProducts)
                {
                    if (identifier is CategoryPage)
                    {
                        blockedProducts.AddRange(identifier.Descendants<Product>());
                    }
                    else if (identifier is ProductTag)
                    {
                        var products = GetAllProducts().Where(p => p?.Tag != null && p.Tag.Any(t => t.Id == ((ProductTag)identifier).Id));
                        blockedProducts.AddRange(products);
                    }
                }
            }

            return blockedProducts.Any(p => p.Id == productId);
        }
    }   
}