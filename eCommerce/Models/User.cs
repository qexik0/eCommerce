using MongoDB.Bson.Serialization.Attributes;

namespace eCommerce.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        public List<UserRating> UserRatings { get; set; }
        public List<UserReview> UserReviews { get; set; }
    }
}
