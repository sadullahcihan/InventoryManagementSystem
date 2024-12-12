using InventoryManagementSystem.Domain.Entities;
using MongoDB.Driver;

namespace InventoryManagementSystem.Infrastructure.Persistence
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }
        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");

    }
}
