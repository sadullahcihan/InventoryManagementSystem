using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructures.Persistence
{
    public interface IMongoCollection<T>
    {
        Task InsertOneAsync(T document);

        Task ReplaceOneAsync(Expression<Func<T, bool>> filter, T document);

        Task DeleteOneAsync(Expression<Func<T, bool>> filter);

        Task<T?> FindOneAsync(Expression<Func<T, bool>> filter);

        Task<List<T>> FindAsync(Expression<Func<T, bool>> filter);

        Task<long> CountDocumentsAsync(Expression<Func<T, bool>> filter);
    }
}
