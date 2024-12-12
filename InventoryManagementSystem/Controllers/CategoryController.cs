using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly CategoriesService _categoriesService;

    public CategoriesController(CategoriesService categoriesService) =>
        _categoriesService = categoriesService;

    [HttpGet]
    public async Task<List<Category>> GetAllCategoriesAsync() =>
        await _categoriesService.GetAsync();

    [HttpGet("{id:length(1)}")]
    public async Task<ActionResult<Category>> GetByIdAsync(int id)
    {
        var category = await _categoriesService.GetAsync(id);

        if (category is null)
        {
            return NotFound();
        }

        return category;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategoryAsync(Category newCategory)
    {
        await _categoriesService.CreateAsync(newCategory);

        return CreatedAtAction(nameof(GetAllCategoriesAsync), new { id = newCategory.Id }, newCategory);
    }

    [HttpPut("{id:length(1)}")]
    public async Task<IActionResult> UpdateCategoryAsync(int id, Category updatedCategory)
    {
        var category = await _categoriesService.GetAsync(id);

        if (category is null)
        {
            return NotFound();
        }

        updatedCategory.Id = category.Id;

        await _categoriesService.UpdateAsync(id, updatedCategory);

        return NoContent();
    }

    [HttpDelete("{id:length(1)}")]
    public async Task<IActionResult> DeleteCategoryByIdAsync(int id)
    {
        var category = await _categoriesService.GetAsync(id);

        if (category is null)
        {
            return NotFound();
        }

        await _categoriesService.RemoveAsync(id);

        return NoContent();
    }
}