using InventoryManagementSystem.Controllers;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace InventoryManagementSystem.Tests
{
    public class ProductsControllerTests
    {
        private readonly Mock<ILogger<ProductsController>> _mockLogger;
        private readonly Mock<ProductsService> _mockProductsService;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockLogger = new Mock<ILogger<ProductsController>>();
            _mockProductsService = new Mock<ProductsService>();
            _controller = new ProductsController(_mockProductsService.Object, _mockLogger.Object);
        }

        // Test: GetAllProductsAsync
        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnListOfProducts_WhenProductsExist()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product One", Price = 10.0M },
                new Product { Id = 2, Name = "Product Two", Price = 20.0M }
            };
            _mockProductsService.Setup(service => service.GetAsync()).ReturnsAsync(products);

            var result = await _controller.GetAllProductsAsync();

            var actionResult = Assert.IsType<List<Product>>(result);
            Assert.Equal(2, actionResult.Count);
        }

        // Test: GetByIdAsync
        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            var product = new Product { Id = 1, Name = "Product One", Price = 10.0M };
            _mockProductsService.Setup(service => service.GetAsync(1)).ReturnsAsync(product);

            var result = await _controller.GetByIdAsync(1);

            var actionResult = Assert.IsType<ActionResult<Product>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(product.Name, returnProduct.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            _mockProductsService.Setup(service => service.GetAsync(1)).ReturnsAsync((Product)null);

            var result = await _controller.GetByIdAsync(1);

            var actionResult = Assert.IsType<ActionResult<Product>>(result);
            var notFoundResult = Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        // Test: CreateProductAsync
        [Fact]
        public async Task CreateProductAsync_ShouldReturnCreatedResult_WhenProductIsValid()
        {
            var newProduct = new Product { Id = 1, Name = "New Product", Price = 30.0M };
            _mockProductsService.Setup(service => service.CreateAsync(newProduct)).Returns(Task.CompletedTask);

            var result = await _controller.CreateProductAsync(newProduct);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetAllProductsAsync", createdResult.ActionName);
            Assert.Equal(1, createdResult.RouteValues["id"]);
        }

        // Test: UpdateProductAsync
        [Fact]
        public async Task UpdateProductAsync_ShouldReturnNoContent_WhenProductExists()
        {
            var updatedProduct = new Product { Id = 1, Name = "Updated Product", Price = 40.0M };
            var existingProduct = new Product { Id = 1, Name = "Product One", Price = 10.0M };
            _mockProductsService.Setup(service => service.GetAsync(1)).ReturnsAsync(existingProduct);
            _mockProductsService.Setup(service => service.UpdateAsync(1, updatedProduct)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateProductAsync(1, updatedProduct);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            var updatedProduct = new Product { Id = 1, Name = "Updated Product", Price = 40.0M };
            _mockProductsService.Setup(service => service.GetAsync(1)).ReturnsAsync((Product)null);

            var result = await _controller.UpdateProductAsync(1, updatedProduct);

            Assert.IsType<NotFoundResult>(result);
        }

        // Test: DeleteProductByIdAsync
        [Fact]
        public async Task DeleteProductByIdAsync_ShouldReturnNoContent_WhenProductExists()
        {
            var product = new Product { Id = 1, Name = "Product One", Price = 10.0M };
            _mockProductsService.Setup(service => service.GetAsync(1)).ReturnsAsync(product);
            _mockProductsService.Setup(service => service.RemoveAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteProductByIdAsync(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProductByIdAsync_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            _mockProductsService.Setup(service => service.GetAsync(1)).ReturnsAsync((Product)null);

            var result = await _controller.DeleteProductByIdAsync(1);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
