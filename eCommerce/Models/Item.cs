using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eCommerce.Models
{
    [BsonIgnoreExtraElements]
    public class Item
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string CategoryId { get; set; }

        public string Description { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? Price { get; set; }

        public string Seller { get; set; }

        public string ImageLink { get; set; }

        public string Size { get; set; }

        public string Color { get; set; }

        public string Spec { get; set; }

        [BsonIgnore]
        public double? AvgRating { get
            {
                if (ItemRatings == null || ItemRatings.Count == 0)
                {
                    return null;
                }
                else
                {
                    return (double) ItemRatings.Sum(x => x.Rating) / ItemRatings.Count;
                }
            }
        }

        public List<ItemRating> ItemRatings { get; set; }
        public List<ItemReview> ItemReviews { get; set; }
    }
}
