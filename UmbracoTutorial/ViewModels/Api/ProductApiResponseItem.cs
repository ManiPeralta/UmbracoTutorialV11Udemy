namespace UmbracoTutorial.ViewModels.Api
{
    public class ProductApiResponseItem
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? ProductSKU { get; set; }
        public List<string>? Categories { get; set; }
        public string? Description { get; set; }
    }
}
