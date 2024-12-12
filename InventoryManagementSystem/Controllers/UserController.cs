//using InventoryManagementSystem.Domain.Entities;
////using InventoryManagementSystem.Infrastructure.Services;
////using InventoryManagementSystem.Application.Exceptions;
////using InventoryManagementSystem.Infrastructure.Repositories;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using InventoryManagementSystem.Constants;
//using Microsoft.AspNetCore.Identity;
//using System.ComponentModel.DataAnnotations;

//namespace InventoryManagementSystem.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class UserController : ControllerBase
//    {
//        private readonly IUserRepository _userRepository;
//        private readonly IPasswordHasher _passwordHasher;
//        private readonly ITokenService _tokenService;

//        public UserController(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
//        {
//            _userRepository = userRepository;
//            _passwordHasher = passwordHasher;
//            _tokenService = tokenService;
//        }

//        // POST /api/users/register
//        [HttpPost("register")]
//        public async Task<IActionResult> Register([FromBody] User user)
//        {
//            if (await _userRepository.ExistsByUsernameAsync(user.Username))
//                throw new ValidationException("Username is already taken.");

//            if (await _userRepository.ExistsByEmailAsync(user.Email))
//                throw new ValidationException("Email is already in use.");

//            user.Password = _passwordHasher.HashPassword(user.Password);
//            user.Role = UserRole.Viewer; // Default role
//            await _userRepository.AddAsync(user);

//            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
//        }

//        // POST /api/users/login
//        [HttpPost("login")]
//        public async Task<IActionResult> Login([FromBody] LoginRequest request)
//        {
//            var user = await _userRepository.GetByUsernameAsync(request.Username);
//            if (user == null || !_passwordHasher.VerifyPassword(user.Password, request.Password))
//                throw new UnauthorizedException("Invalid username or password.");

//            var token = _tokenService.GenerateToken(user.Id.ToString(), user.Role.ToString());
//            return Ok(new { Token = token });
//        }

//        // GET /api/users/{id} - Only accessible by Admin
//        [HttpGet("{id}")]
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> GetUserById(int id)
//        {
//            var user = await _userRepository.GetByIdAsync(id);
//            if (user == null)
//                throw new NotFoundException("User not found.");

//            return Ok(user);
//        }

//        // GET /api/users - Only accessible by Admin
//        [HttpGet]
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> GetAllUsers()
//        {
//            var users = await _userRepository.GetAllAsync();
//            return Ok(users);
//        }

//        // PUT /api/users/{id} - Only accessible by Admin
//        [HttpPut("{id}")]
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
//        {
//            var user = await _userRepository.GetByIdAsync(id);
//            if (user == null)
//                throw new NotFoundException("User not found.");

//            user.Username = updatedUser.Username ?? user.Username;
//            user.Email = updatedUser.Email ?? user.Email;
//            user.Role = updatedUser.Role;

//            await _userRepository.UpdateAsync(user);
//            return NoContent();
//        }

//        // DELETE /api/users/{id} - Only accessible by Admin
//        [HttpDelete("{id}")]
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> DeleteUser(int id)
//        {
//            var user = await _userRepository.GetByIdAsync(id);
//            if (user == null)
//                throw new NotFoundException("User not found.");

//            await _userRepository.DeleteAsync(user);
//            return NoContent();
//        }
//    }

//    public class LoginRequest
//    {
//        public string Username { get; set; } = string.Empty;
//        public string Password { get; set; } = string.Empty;
//    }
//}
