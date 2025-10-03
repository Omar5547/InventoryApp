namespace InventoryApp.ViewModel
{
    public class DashboardViewModel
    {
        public int OrdersCount { get; set; }
        public int ProductsCount { get; set; }
        public int CustomersCount { get; set; }
        public int SuppliersCount { get; set; }
        public List<int> MonthlySales { get; set; } = new List<int>();
    }
}
