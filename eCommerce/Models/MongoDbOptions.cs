namespace eCommerce.Models
{
    public class MongoDbOptions
    {
        public const string SectionName = "MongoDbOptions";
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string UserCollectionName { get; set; }

        public string ItemCollectionName { get; set; }

        public string CategoryCollectionName { get; set; }
    }
}
