using InventoryManagementSystem.Controllers;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace InventoryManagementSystem.Tests
{
    public class CategoriesControllerTests
    {
        private readonly Mock<ILogger<CategoriesController>> _mockLogger;
        private readonly Mock<CategoriesService> _mockCategoriesService;
        private readonly CategoriesController _controller;

        public CategoriesControllerTests()
        {
            _mockLogger = new Mock<ILogger<CategoriesController>>();
            _mockCategoriesService = new Mock<CategoriesService>();
            _controller = new CategoriesController(_mockCategoriesService.Object, _mockLogger.Object);
        }

        // Test: GetAllCategoriesAsync
        [Fact]
        public async Task GetAllCategoriesAsync_ShouldReturnListOfCategories_WhenCategoriesExist()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category One" },
                new Category { Id = 2, Name = "Category Two" }
            };
            _mockCategoriesService.Setup(service => service.GetAsync()).ReturnsAsync(categories);

            var result = await _controller.GetAllCategoriesAsync();

            var actionResult = Assert.IsType<List<Category>>(result);
            Assert.Equal(2, actionResult.Count);
        }

        // Test: GetByIdAsync
        [Fact]
        public async Task GetByIdAsync_ShouldReturnCategory_WhenCategoryExists()
        {
            var category = new Category { Id = 1, Name = "Category One" };
            _mockCategoriesService.Setup(service => service.GetAsync(1)).ReturnsAsync(category);

            var result = await _controller.GetByIdAsync(1);

            var actionResult = Assert.IsType<ActionResult<Category>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnCategory = Assert.IsType<Category>(okResult.Value);
            Assert.Equal(category.Name, returnCategory.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNotFound_WhenCategoryDoesNotExist()
        {
            _mockCategoriesService.Setup(service => service.GetAsync(1)).ReturnsAsync((Category)null);

            var result = await _controller.GetByIdAsync(1);

            var actionResult = Assert.IsType<ActionResult<Category>>(result);
            var notFoundResult = Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        // Test: CreateCategoryAsync
        [Fact]
        public async Task CreateCategoryAsync_ShouldReturnCreatedResult_WhenCategoryIsValid()
        {
            var newCategory = new Category { Id = 1, Name = "New Category" };
            _mockCategoriesService.Setup(service => service.CreateAsync(newCategory)).Returns(Task.CompletedTask);

            var result = await _controller.CreateCategoryAsync(newCategory);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetAllCategoriesAsync", createdResult.ActionName);
            Assert.Equal(1, createdResult.RouteValues["id"]);
        }

        // Test: UpdateCategoryAsync
        [Fact]
        public async Task UpdateCategoryAsync_ShouldReturnNoContent_WhenCategoryExists()
        {
            var updatedCategory = new Category { Id = 1, Name = "Updated Category" };
            var existingCategory = new Category { Id = 1, Name = "Category One" };
            _mockCategoriesService.Setup(service => service.GetAsync(1)).ReturnsAsync(existingCategory);
            _mockCategoriesService.Setup(service => service.UpdateAsync(1, updatedCategory)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateCategoryAsync(1, updatedCategory);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldReturnNotFound_WhenCategoryDoesNotExist()
        {
            var updatedCategory = new Category { Id = 1, Name = "Updated Category" };
            _mockCategoriesService.Setup(service => service.GetAsync(1)).ReturnsAsync((Category)null);

            var result = await _controller.UpdateCategoryAsync(1, updatedCategory);

            Assert.IsType<NotFoundResult>(result);
        }

        // Test: DeleteCategoryByIdAsync
        [Fact]
        public async Task DeleteCategoryByIdAsync_ShouldReturnNoContent_WhenCategoryExists()
        {
            var category = new Category { Id = 1, Name = "Category One" };
            _mockCategoriesService.Setup(service => service.GetAsync(1)).ReturnsAsync(category);
            _mockCategoriesService.Setup(service => service.RemoveAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteCategoryByIdAsync(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCategoryByIdAsync_ShouldReturnNotFound_WhenCategoryDoesNotExist()
        {
            _mockCategoriesService.Setup(service => service.GetAsync(1)).ReturnsAsync((Category)null);

            var result = await _controller.DeleteCategoryByIdAsync(1);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
