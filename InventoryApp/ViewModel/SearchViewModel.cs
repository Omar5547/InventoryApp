namespace InventoryApp.ViewModel
{
    public class SearchViewModel
    {
        public string? Query { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public IEnumerable<ProductViewModel> Results { get; set; } = Enumerable.Empty<ProductViewModel>();
        public IEnumerable<CategoryViewModel> Categories { get; set; } = Enumerable.Empty<CategoryViewModel>();
        public int Total => Results?.Count() ?? 0;
    }
}
