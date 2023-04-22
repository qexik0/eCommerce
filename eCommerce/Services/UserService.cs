using eCommerce.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace eCommerce.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IOptions<MongoDbOptions> mongoDbOptions)
        {
            var client = new MongoClient(mongoDbOptions.Value.ConnectionString);
            var database = client.GetDatabase(mongoDbOptions.Value.DatabaseName);

            _users = database.GetCollection<User>(mongoDbOptions.Value.UserCollectionName);
        }

        public async Task<User> Create(string username, string password, bool isAdmin)
        {
            var user = new User { Username = username, Password = password, IsAdmin = isAdmin };
            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task<User> GetByUsername(string username)
        {
            return await _users.FindAsync<User>(user => user.Username == username).Result.FirstOrDefaultAsync();
        }
        public async Task<User> GetById(string id)
        {
            return await _users.FindAsync<User>(user => user.Id == id).Result.FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetAll()
        {
            var q = await _users.FindAsync<User>(_ => true);
            return q.ToList();
        }

        public async void DeleteByUsername(string username)
        {
            await _users.FindOneAndDeleteAsync(user => user.Username == username);
        }

        public async Task UpdateRatings(User user)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, user.Id);
            var update = Builders<User>.Update.Set(x => x.UserRatings, user.UserRatings);
            await _users.UpdateOneAsync(filter, update);
        }
        public async void UpdateReviews(User user)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, user.Id);
            var update = Builders<User>.Update.Set(x => x.UserReviews, user.UserReviews);
            await _users.UpdateOneAsync(filter, update);
        }
    }
}
