using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using InventoryManagementSystem.Domain.Entities;
using MongoDB.Bson;

namespace InventoryManagementSystem.Infrastructures.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(IMongoDatabase database)
        {
            _products = database.GetCollection<Product>("Products");
        }

        public async Task AddAsync(Product product)
        {
            await _products.InsertOneAsync(product);
        }

        public async Task UpdateAsync(Product product)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
            await _products.ReplaceOneAsync(filter, product);
        }

        public async Task DeleteAsync(Product product)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
            await _products.DeleteOneAsync(filter);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            return await _products.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetAllAsync(int? categoryId = null)
        {
            var filter = categoryId.HasValue
                ? Builders<Product>.Filter.Eq(p => p.CategoryId, categoryId.Value)
                : Builders<Product>.Filter.Empty;

            return await _products.Find(filter).ToListAsync();
        }
    }
}
