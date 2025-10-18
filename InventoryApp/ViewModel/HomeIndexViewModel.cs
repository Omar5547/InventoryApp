namespace InventoryApp.ViewModel
{
    public class HomeIndexViewModel
    {
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
        public IEnumerable<ProductViewModel> LatestProducts { get; set; }
    }

   
}
