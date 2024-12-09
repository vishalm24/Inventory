namespace Inventory.Models.ViewModel
{
    public class ProductCategorySummaryViewModel
    {
        public int ProductId {  get; set; }
        public string ProductName { get; set; } = default!;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
