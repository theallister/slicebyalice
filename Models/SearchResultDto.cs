namespace SliceByAlice.Models
{
    public class SearchResultDto
    {
        public int Id { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}