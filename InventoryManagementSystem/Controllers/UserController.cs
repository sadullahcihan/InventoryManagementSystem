using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UsersService _usersService;

    public UsersController(UsersService usersService) =>
        _usersService = usersService;

    [HttpGet]
    public async Task<List<User>> GetAllUsersAsync() =>
        await _usersService.GetAllUsersAsync();

    [HttpGet("{id:length(1)}")]
    public async Task<ActionResult<User>> GetUserByIdAsync(int id)
    {
        var user = await _usersService.GetUserByIdAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync(User newUser)
    {
        await _usersService.CreateUserAsync(newUser);

        return CreatedAtAction(nameof(GetUserByIdAsync), new { id = newUser.Id }, newUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult<User>> LoginAsync([FromBody] LoginRequestModel request)
    {
        var user = await _usersService.LoginAsync(request.Email, request.Password);

        if (user is null)
        {
            return Unauthorized("Invalid email or password.");
        }

        return Ok(user);
    }

    [HttpPut("{id:length(1)}")]
    public async Task<IActionResult> UpdateUserAsync(int id, User updatedUser)
    {
        var user = await _usersService.GetUserByIdAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        updatedUser.Id = user.Id;

        await _usersService.UpdateUserAsync(id, updatedUser);

        return NoContent();
    }

    [HttpDelete("{id:length(1)}")]
    public async Task<IActionResult> DeleteUserByIdAsync(int id)
    {
        var user = await _usersService.GetUserByIdAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        await _usersService.DeleteUserAsync(id);

        return NoContent();
    }
}