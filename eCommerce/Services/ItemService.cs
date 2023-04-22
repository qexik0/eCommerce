using eCommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace eCommerce.Services
{
    public class ItemService
    {
        private readonly IMongoCollection<Item> _items;
        private readonly IMongoCollection<Category> _categories;

        public ItemService(IOptions<MongoDbOptions> mongoDbOptions)
        {
            var client = new MongoClient(mongoDbOptions.Value.ConnectionString);
            var database = client.GetDatabase(mongoDbOptions.Value.DatabaseName);

            _categories = database.GetCollection<Category>(mongoDbOptions.Value.CategoryCollectionName);
            _items = database.GetCollection<Item>(mongoDbOptions.Value.ItemCollectionName);
        }

        public async Task<Item> Create(Item item)
        {
            await _items.InsertOneAsync(item);
            return item;
        }

        public async Task<List<Item>> GetAll()
        {
            var q = await _items.FindAsync(x => true);
            return q.ToList();
        }

        public async Task<List<Category>> GetCategories()
        {
            var q = await _categories.FindAsync(x => true);
            return q.ToList();
        }

        public async Task CreateCategory(string name)
        {
            await _categories.InsertOneAsync(new Category {  Name = name });
        }

        public async void RemoveCategory(string name)
        {
            await _categories.FindOneAndDeleteAsync(x => x.Name == name);
        }

        public async Task<Item> GetById(string id)
        {
            var q = await _items.FindAsync(x => x.Id == id);
            return q.FirstOrDefault();
        }

        public async Task<string> GetCategoryName(string id)
        {
            var q = await _categories.FindAsync(x => x.Id == id);
            return q.First().Name;
        }

        public async Task UpdateRatings(Item item)
        {
            var filter = Builders<Item>.Filter.Eq(x => x.Id, item.Id);
            var update = Builders<Item>.Update.Set(x => x.ItemRatings, item.ItemRatings);
            await _items.UpdateOneAsync(filter, update);
        }

        public async void UpdateReviews(Item item)
        {
            var filter = Builders<Item>.Filter.Eq(x => x.Id, item.Id);
            var update = Builders<Item>.Update.Set(x => x.ItemReviews, item.ItemReviews);
            await _items.UpdateOneAsync(filter, update);
        }

        public async Task UpdateItem(Item item)
        {
            var filter = Builders<Item>.Filter.Eq(x => x.Id, item.Id);
            var update = Builders<Item>.Update.Set(x => x.Name, item.Name)
                .Set(x => x.Description, item.Description)
                .Set(x => x.Price, item.Price)
                .Set(x => x.Seller, item.Seller)
                .Set(x => x.CategoryId, item.CategoryId)
                .Set(x => x.ImageLink, item.ImageLink)
                .Set(x => x.Size, item.Size)
                .Set(x => x.Spec, item.Spec)
                .Set(x => x.Color, item.Color);
            await _items.UpdateOneAsync(filter, update);
        }

        public async void RemoveItem(Item item)
        {
            var filter = Builders<Item>.Filter.Eq(x => x.Id, item.Id);
            await _items.DeleteOneAsync(filter);
        }
    }
}
