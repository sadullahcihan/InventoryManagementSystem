using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace InventoryManagementSystem.Services
{
    public class ProductsService
    {
        private readonly IMongoCollection<Product> _productsCollection;

        public ProductsService(
            IOptions<InventoryManagementSystemDatabaseSettings> inventoryManagementSystemDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                inventoryManagementSystemDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                inventoryManagementSystemDatabaseSettings.Value.DatabaseName);

            _productsCollection = mongoDatabase.GetCollection<Product>(
                inventoryManagementSystemDatabaseSettings.Value.ProductCollectionName);
        }

        public async Task<List<Product>> GetAsync() =>
            await _productsCollection.Find(_ => true).ToListAsync();

        public async Task<Product?> GetAsync(int id) =>
            await _productsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Product newProduct) =>
            await _productsCollection.InsertOneAsync(newProduct);

        public async Task UpdateAsync(int id, Product updatedProduct) =>
            await _productsCollection.ReplaceOneAsync(x => x.Id == id, updatedProduct);

        public async Task RemoveAsync(int id) =>
            await _productsCollection.DeleteOneAsync(x => x.Id == id);
    }
}
