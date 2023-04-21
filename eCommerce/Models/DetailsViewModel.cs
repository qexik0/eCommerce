namespace eCommerce.Models
{
    public class DetailsViewModel
    {
        public Item Item { get; set; }
        public int? Rate { get; set; }
        public int? UserRating { get; set; }
        public string ItemId { get; set; }
        public List<string> ReviewUsernames { get; set; }
        public string ReviewText { get; set; }
    }
}
