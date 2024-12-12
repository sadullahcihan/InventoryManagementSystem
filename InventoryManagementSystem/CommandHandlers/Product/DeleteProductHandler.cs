using InventoryManagementSystem.CommandHandlers.Product.Interfaces;
using InventoryManagementSystem.Infrastructures.Repositories;
using System;

namespace InventoryManagementSystem.CommandHandlers.Product
{
    public class DeleteProductHandler : IDeleteProductHandler
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(DeleteProductCommand command)
        {
            // Ürünü ID'ye göre al
            var product = await _productRepository.GetByIdAsync(command.Id);

            // Ürün bulunamazsa hata fırlat
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {command.Id} not found.");

            // Ürünü sil
            await _productRepository.DeleteAsync(product);
        }
    }
}
