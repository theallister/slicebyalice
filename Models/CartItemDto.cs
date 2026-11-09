namespace SliceByAlice.Models
{
    public class CartItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string? ProductType { get; set; }
        public string? ImageUrl { get; set; }
        public string? Name { get; set; }
        public int Price { get; set; }
        public int? TwoForPrice { get; set; }
        public string? Url { get; set; }
        public int InStock { get; set; }
    }  
}