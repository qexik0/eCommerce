using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eCommerce.Models
{
    public class Review
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Text { get; set; }
    }
}
