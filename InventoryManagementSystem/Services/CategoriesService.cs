using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace InventoryManagementSystem.Services
{
    public class CategoriesService
    {
        private readonly IMongoCollection<Category> _categorysCollection;

        public CategoriesService(
            IOptions<InventoryManagementSystemDatabaseSettings> inventoryManagementSystemDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                inventoryManagementSystemDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                inventoryManagementSystemDatabaseSettings.Value.DatabaseName);

            _categorysCollection = mongoDatabase.GetCollection<Category>(
                inventoryManagementSystemDatabaseSettings.Value.CategoryCollectionName);
        }

        public async Task<List<Category>> GetAsync() =>
            await _categorysCollection.Find(_ => true).ToListAsync();

        public async Task<Category?> GetAsync(int id) =>
            await _categorysCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Category newCategory) =>
            await _categorysCollection.InsertOneAsync(newCategory);

        public async Task UpdateAsync(int id, Category updatedCategory) =>
            await _categorysCollection.ReplaceOneAsync(x => x.Id == id, updatedCategory);

        public async Task RemoveAsync(int id) =>
            await _categorysCollection.DeleteOneAsync(x => x.Id == id);
    }
}
