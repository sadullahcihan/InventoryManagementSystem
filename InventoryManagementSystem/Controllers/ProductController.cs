using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductsService _productsService;

    public ProductsController(ProductsService productsService) =>
        _productsService = productsService;

    [HttpGet]
    public async Task<List<Product>> GetAllProductsAsync() =>
        await _productsService.GetAsync();

    [HttpGet("{id:length(1)}")]
    public async Task<ActionResult<Product>> GetByIdAsync(int id)
    {
        var product = await _productsService.GetAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        return product;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync(Product newProduct)
    {
        await _productsService.CreateAsync(newProduct);

        return CreatedAtAction(nameof(GetAllProductsAsync), new { id = newProduct.Id }, newProduct);
    }

    [HttpPut("{id:length(1)}")]
    public async Task<IActionResult> UpdateProductAsync(int id, Product updatedProduct)
    {
        var product = await _productsService.GetAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        updatedProduct.Id = product.Id;

        await _productsService.UpdateAsync(id, updatedProduct);

        return NoContent();
    }

    [HttpDelete("{id:length(1)}")]
    public async Task<IActionResult> DeleteProductByIdAsync(int id)
    {
        var product = await _productsService.GetAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        await _productsService.RemoveAsync(id);

        return NoContent();
    }
}