namespace InventoryApp.ViewModel
{
    public class CustomerViewModel
    {
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public decimal? Discount { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
