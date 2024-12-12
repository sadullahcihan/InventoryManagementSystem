using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Infrastructures.Repositories
{
    public interface IProductRepository
    {
        // Yeni ürün ekleme
        Task AddAsync(Product product);

        // Mevcut ürün güncelleme
        Task UpdateAsync(Product product);

        // Ürünü ID'ye göre silme
        Task DeleteAsync(Product product);

        // Belirli bir ürünü ID'ye göre getirme
        Task<Product?> GetByIdAsync(int id);

        // Filtreleme destekli tüm ürünleri getirme
        Task<List<Product>> GetAllAsync(int? categoryId = null);
    }
}
