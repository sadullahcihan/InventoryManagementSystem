using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InventoryManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly CategoriesService _categoriesService;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(CategoriesService categoriesService, ILogger<CategoriesController> logger)
    {
        _categoriesService = categoriesService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        _logger.LogInformation("Fetching all categories.");
        var categories = await _categoriesService.GetAsync();
        _logger.LogInformation($"Returned {categories.Count} categories.");
        return categories;
    }

    [HttpGet("{id:length(1)}")]
    public async Task<ActionResult<Category>> GetByIdAsync(int id)
    {
        _logger.LogInformation($"Fetching category with ID: {id}");
        var category = await _categoriesService.GetAsync(id);

        if (category is null)
        {
            _logger.LogWarning($"Category with ID {id} not found.");
            return NotFound();
        }

        _logger.LogInformation($"Category with ID {id} found.");
        return category;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategoryAsync(Category newCategory)
    {
        _logger.LogInformation($"Attempting to create new category with name: {newCategory.Name}");

        try
        {
            await _categoriesService.CreateAsync(newCategory);
            _logger.LogInformation($"Category with name {newCategory.Name} successfully created.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating category with name {newCategory.Name}: {ex.Message}");
            return StatusCode(500, "Internal server error.");
        }

        return CreatedAtAction(nameof(GetAllCategoriesAsync), new { id = newCategory.Id }, newCategory);
    }

    [HttpPut("{id:length(1)}")]
    public async Task<IActionResult> UpdateCategoryAsync(int id, Category updatedCategory)
    {
        _logger.LogInformation($"Attempting to update category with ID: {id}");

        var category = await _categoriesService.GetAsync(id);

        if (category is null)
        {
            _logger.LogWarning($"Category with ID {id} not found for update.");
            return NotFound();
        }

        updatedCategory.Id = category.Id;
        await _categoriesService.UpdateAsync(id, updatedCategory);

        _logger.LogInformation($"Category with ID {id} successfully updated.");
        return NoContent();
    }

    [HttpDelete("{id:length(1)}")]
    public async Task<IActionResult> DeleteCategoryByIdAsync(int id)
    {
        _logger.LogInformation($"Attempting to delete category with ID: {id}");

        var category = await _categoriesService.GetAsync(id);

        if (category is null)
        {
            _logger.LogWarning($"Category with ID {id} not found for deletion.");
            return NotFound();
        }

        await _categoriesService.RemoveAsync(id);

        _logger.LogInformation($"Category with ID {id} successfully deleted.");
        return NoContent();
    }
}
