using InventoryManagementSystem.CommandHandlers.Product.Interfaces;
using InventoryManagementSystem.Infrastructures.Repositories;

namespace InventoryManagementSystem.CommandHandlers.Product
{
    public class UpdateProductHandler : IUpdateProductHandler
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(UpdateProductCommand command)
        {
            var product = await _productRepository.GetByIdAsync(command.Id);
            if (product == null)
                throw new Exception("Product not found.");

            product.Name = command.Name ?? product.Name;
            product.Description = command.Description ?? product.Description;
            product.Price = command.Price ?? product.Price;
            product.Quantity = command.Quantity ?? product.Quantity;
            product.CategoryId = command.CategoryId ?? product.CategoryId;

            await _productRepository.UpdateAsync(product);
        }
    }

}
