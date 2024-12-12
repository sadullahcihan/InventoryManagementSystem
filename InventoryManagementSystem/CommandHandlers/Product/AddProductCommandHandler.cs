using InventoryManagementSystem.CommandHandlers.Product.Interfaces;
using InventoryManagementSystem.Infrastructures.Repositories;

namespace InventoryManagementSystem.CommandHandlers.Product
{
    public class AddProductCommandHandler : IAddProductHandler
    {
        private readonly IProductRepository _productRepository;

        public AddProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(AddProductCommand command)
        {
            var product = new Domain.Entities.Product
            {
                Id = 1,
                Name = command.Name,
                Description = command.Description,
                Price = command.Price,
                Quantity = command.Quantity,
                CategoryId = command.CategoryId
            };

            await _productRepository.AddAsync(product);
        }
    }

}
