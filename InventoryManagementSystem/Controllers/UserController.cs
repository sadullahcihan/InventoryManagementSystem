using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InventoryManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UsersService _usersService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(UsersService usersService, ILogger<UsersController> logger)
    {
        _usersService = usersService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<List<User>> GetAllUsersAsync()
    {
        _logger.LogInformation("Fetching all users.");
        var users = await _usersService.GetAllUsersAsync();
        _logger.LogInformation($"Returned {users.Count} users.");
        return users;
    }

    [HttpGet("{id:length(1)}")]
    public async Task<ActionResult<User>> GetUserByIdAsync(int id)
    {
        _logger.LogInformation($"Fetching user with ID: {id}");
        var user = await _usersService.GetUserByIdAsync(id);

        if (user is null)
        {
            _logger.LogWarning($"User with ID {id} not found.");
            return NotFound();
        }

        _logger.LogInformation($"User with ID {id} found.");
        return user;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync(User newUser)
    {
        _logger.LogInformation($"Attempting to register user with email: {newUser.Email}");

        try
        {
            await _usersService.CreateUserAsync(newUser);
            _logger.LogInformation($"User with email {newUser.Email} successfully registered.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error registering user with email {newUser.Email}: {ex.Message}");
            return StatusCode(500, "Internal server error.");
        }

        return CreatedAtAction(nameof(GetUserByIdAsync), new { id = newUser.Id }, newUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult<User>> LoginAsync([FromBody] LoginRequestModel request)
    {
        _logger.LogInformation($"Login attempt for email: {request.Email}");

        var user = await _usersService.LoginAsync(request.Email, request.Password);

        if (user is null)
        {
            _logger.LogWarning($"Failed login attempt for email: {request.Email}");
            return Unauthorized("Invalid email or password.");
        }

        _logger.LogInformation($"User with email {request.Email} logged in successfully.");
        return Ok(user);
    }

    [HttpPut("{id:length(1)}")]
    public async Task<IActionResult> UpdateUserAsync(int id, User updatedUser)
    {
        _logger.LogInformation($"Attempting to update user with ID: {id}");

        var user = await _usersService.GetUserByIdAsync(id);

        if (user is null)
        {
            _logger.LogWarning($"User with ID {id} not found for update.");
            return NotFound();
        }

        updatedUser.Id = user.Id;
        await _usersService.UpdateUserAsync(id, updatedUser);

        _logger.LogInformation($"User with ID {id} successfully updated.");
        return NoContent();
    }

    [HttpDelete("{id:length(1)}")]
    public async Task<IActionResult> DeleteUserByIdAsync(int id)
    {
        _logger.LogInformation($"Attempting to delete user with ID: {id}");

        var user = await _usersService.GetUserByIdAsync(id);

        if (user is null)
        {
            _logger.LogWarning($"User with ID {id} not found for deletion.");
            return NotFound();
        }

        await _usersService.DeleteUserAsync(id);

        _logger.LogInformation($"User with ID {id} successfully deleted.");
        return NoContent();
    }
}
