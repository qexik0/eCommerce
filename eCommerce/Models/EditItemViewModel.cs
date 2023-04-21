namespace eCommerce.Models
{
    public class EditItemViewModel
    {
        public Item Item { get; set; }
        public List<Category> Categories { get; set; }
        public string ItemId { get; set; }
    }
}
