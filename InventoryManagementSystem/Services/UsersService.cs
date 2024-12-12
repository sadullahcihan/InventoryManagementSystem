using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Infrastructures.Persistence;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace InventoryManagementSystem.Services
{
    public class UsersService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UsersService(
            IOptions<InventoryManagementSystemDatabaseSettings> inventoryManagementSystemDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                inventoryManagementSystemDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                inventoryManagementSystemDatabaseSettings.Value.DatabaseName);

            _usersCollection = mongoDatabase.GetCollection<User>(
                inventoryManagementSystemDatabaseSettings.Value.UserCollectionName);
        }

        // Yeni kullanıcı kaydı
        public async Task CreateUserAsync(User newUser)
        {
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(password);
            await _usersCollection.InsertOneAsync(newUser);
        }

        // Kullanıcı girişi (JWT token alma işlemi bu seviyede yapılmaz, ayrı bir servis önerilir)
        public async Task<User?> LoginAsync(string email, string password)
        {
            return await _usersCollection.Find(x => x.Email == email && x.Password == password).FirstOrDefaultAsync();
        }

        // Kullanıcı bilgilerini ID'ye göre getirme (Sadece Admin)
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        // Tüm kullanıcıları listeleme (Sadece Admin)
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _usersCollection.Find(_ => true).ToListAsync();
        }

        // Kullanıcı bilgilerini güncelleme (Sadece Admin)
        public async Task UpdateUserAsync(int id, User updatedUser)
        {
            await _usersCollection.ReplaceOneAsync(x => x.Id == id, updatedUser);
        }

        // Kullanıcı silme (Sadece Admin)
        public async Task DeleteUserAsync(int id)
        {
            await _usersCollection.DeleteOneAsync(x => x.Id == id);
        }
    }
}
