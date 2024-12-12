using InventoryManagementSystem.Controllers;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace InventoryManagementSystem.Tests
{
    public class UsersControllerTests
    {
        private readonly Mock<ILogger<UsersController>> _mockLogger;
        private readonly Mock<UsersService> _mockUsersService;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockLogger = new Mock<ILogger<UsersController>>();
            _mockUsersService = new Mock<UsersService>();
            _controller = new UsersController(_mockUsersService.Object, _mockLogger.Object);
        }

        // Test: GetAllUsersAsync
        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnListOfUsers_WhenUsersExist()
        {
            var users = new List<User>
            {
                new User { Id = 1, Email = "user1@example.com", Username = "User One" },
                new User { Id = 2, Email = "user2@example.com", Username = "User Two" }
            };
            _mockUsersService.Setup(service => service.GetAllUsersAsync()).ReturnsAsync(users);

            var result = await _controller.GetAllUsersAsync();

            var actionResult = Assert.IsType<List<User>>(result);
            Assert.Equal(2, actionResult.Count);
        }

        // Test: GetUserByIdAsync
        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            var user = new User { Id = 1, Email = "user1@example.com", Username = "User One" };
            _mockUsersService.Setup(service => service.GetUserByIdAsync(1)).ReturnsAsync(user);

            var result = await _controller.GetUserByIdAsync(1);

            var actionResult = Assert.IsType<ActionResult<User>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnUser = Assert.IsType<User>(okResult.Value);
            Assert.Equal(user.Email, returnUser.Email);
        }

        // Test: RegisterUserAsync
        [Fact]
        public async Task RegisterUserAsync_ShouldReturnCreatedResult_WhenUserIsValid()
        {
            var newUser = new User { Id = 1, Email = "newuser@example.com", Username = "New User" };
            _mockUsersService.Setup(service => service.CreateUserAsync(newUser)).Returns(Task.CompletedTask);

            var result = await _controller.RegisterUserAsync(newUser);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetUserByIdAsync", createdResult.ActionName);
            Assert.Equal(1, createdResult.RouteValues["id"]);
        }

        // Test: LoginAsync
        [Fact]
        public async Task LoginAsync_ShouldReturnOk_WhenValidCredentials()
        {
            var loginRequest = new LoginRequestModel { Email = "user1@example.com", Password = "password123" };
            var user = new User { Id = 1, Email = "user1@example.com", Username = "User One" };
            _mockUsersService.Setup(service => service.LoginAsync(loginRequest.Email, loginRequest.Password)).ReturnsAsync(user);

            var result = await _controller.LoginAsync(loginRequest);

            var actionResult = Assert.IsType<ActionResult<User>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnUser = Assert.IsType<User>(okResult.Value);
            Assert.Equal(user.Email, returnUser.Email);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnUnauthorized_WhenInvalidCredentials()
        {
            var loginRequest = new LoginRequestModel { Email = "user1@example.com", Password = "wrongpassword" };
            _mockUsersService.Setup(service => service.LoginAsync(loginRequest.Email, loginRequest.Password)).ReturnsAsync((User)null);

            var result = await _controller.LoginAsync(loginRequest);

            var actionResult = Assert.IsType<ActionResult<User>>(result);
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(actionResult.Result);
            Assert.Equal("Invalid email or password.", unauthorizedResult.Value);
        }

        // Test: UpdateUserAsync
        [Fact]
        public async Task UpdateUserAsync_ShouldReturnNoContent_WhenUserExists()
        {
            var updatedUser = new User { Id = 1, Email = "updateduser@example.com", Username = "Updated User" };
            var existingUser = new User { Id = 1, Email = "user1@example.com", Username = "User One" };
            _mockUsersService.Setup(service => service.GetUserByIdAsync(1)).ReturnsAsync(existingUser);
            _mockUsersService.Setup(service => service.UpdateUserAsync(1, updatedUser)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateUserAsync(1, updatedUser);

            Assert.IsType<NoContentResult>(result);
        }

        // Test: DeleteUserByIdAsync
        [Fact]
        public async Task DeleteUserByIdAsync_ShouldReturnNoContent_WhenUserExists()
        {
            var user = new User { Id = 1, Email = "user1@example.com", Username = "User One" };
            _mockUsersService.Setup(service => service.GetUserByIdAsync(1)).ReturnsAsync(user);
            _mockUsersService.Setup(service => service.DeleteUserAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteUserByIdAsync(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            _mockUsersService.Setup(service => service.GetUserByIdAsync(1)).ReturnsAsync((User)null);

            var result = await _controller.DeleteUserByIdAsync(1);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
