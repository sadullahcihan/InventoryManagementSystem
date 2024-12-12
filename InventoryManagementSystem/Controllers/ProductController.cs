using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InventoryManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductsService _productsService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ProductsService productsService, ILogger<ProductsController> logger)
    {
        _productsService = productsService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<List<Product>> GetAllProductsAsync()
    {
        _logger.LogInformation("Fetching all products.");
        var products = await _productsService.GetAsync();
        _logger.LogInformation($"Returned {products.Count} products.");
        return products;
    }

    [HttpGet("{id:length(1)}")]
    public async Task<ActionResult<Product>> GetByIdAsync(int id)
    {
        _logger.LogInformation($"Fetching product with ID: {id}");
        var product = await _productsService.GetAsync(id);

        if (product is null)
        {
            _logger.LogWarning($"Product with ID {id} not found.");
            return NotFound();
        }

        _logger.LogInformation($"Product with ID {id} found.");
        return product;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync(Product newProduct)
    {
        _logger.LogInformation($"Attempting to create new product with name: {newProduct.Name}");

        try
        {
            await _productsService.CreateAsync(newProduct);
            _logger.LogInformation($"Product with name {newProduct.Name} successfully created.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating product with name {newProduct.Name}: {ex.Message}");
            return StatusCode(500, "Internal server error.");
        }

        return CreatedAtAction(nameof(GetAllProductsAsync), new { id = newProduct.Id }, newProduct);
    }

    [HttpPut("{id:length(1)}")]
    public async Task<IActionResult> UpdateProductAsync(int id, Product updatedProduct)
    {
        _logger.LogInformation($"Attempting to update product with ID: {id}");

        var product = await _productsService.GetAsync(id);

        if (product is null)
        {
            _logger.LogWarning($"Product with ID {id} not found for update.");
            return NotFound();
        }

        updatedProduct.Id = product.Id;
        await _productsService.UpdateAsync(id, updatedProduct);

        _logger.LogInformation($"Product with ID {id} successfully updated.");
        return NoContent();
    }

    [HttpDelete("{id:length(1)}")]
    public async Task<IActionResult> DeleteProductByIdAsync(int id)
    {
        _logger.LogInformation($"Attempting to delete product with ID: {id}");

        var product = await _productsService.GetAsync(id);

        if (product is null)
        {
            _logger.LogWarning($"Product with ID {id} not found for deletion.");
            return NotFound();
        }

        await _productsService.RemoveAsync(id);

        _logger.LogInformation($"Product with ID {id} successfully deleted.");
        return NoContent();
    }
}
