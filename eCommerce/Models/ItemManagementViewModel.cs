namespace eCommerce.Models
{
    public class ItemManagementViewModel
    {
        public string CategoryName { get; set; }
        public List<Category> Categories { get; set; }

        public string ItemName { get; set; }

        public string ItemCategory { get; set; }

        public string Description { get; set; }
        public decimal? Price { get; set; }
        public string Seller { get; set; }
        public string ImageLink { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Spec { get; set; }
    }
}
