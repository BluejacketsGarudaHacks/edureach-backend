using Microsoft.AspNetCore.Mvc;
using Backend.Infrastructure.Database;
using Backend.Models;
using Backend.Dtos;
using Backend.Repositories;
using Microsoft.EntityFrameworkCore;
using Backend.Shared.Utils;
using Backend.Validators;
using Backend.Shared.Response;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly JwtUtil _jwtUtil;
        
        public UserController(UserRepository UserRepository, JwtUtil jwtUtil)
        {
            _userRepository = UserRepository;
            _jwtUtil = jwtUtil;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var userId = await _userRepository.GetUserIdByEmailAndPasswordAsync(login.Email, login.Password);
            if (userId == null)
            {
                return NotFound(new FailResponse<string>(null, "User not found"));
            }

            var expires = DateTime.UtcNow.AddDays(7);
            var jwtToken = _jwtUtil.GenerateToken(userId.ToString(), expires);
            return Ok(new SuccessResponse<string>(jwtToken, "Login successful"));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest register)
        {
            var errors = RegisterValidator.Validate(register);
            if (errors.Any())
            {
                return BadRequest(new FailResponse<List<string>>(errors, "Validation failed"));
            }
            
            var user = await _userRepository.GetUserByEmail(register.Email);
            if(user != null) {
                return BadRequest(new FailResponse<string>(null, "Email already registered"));
            }

            var fullName = string.Concat(register.FirstName, " ", register.LastName);
            user = new User 
            {
                Id = Guid.NewGuid(),
                Fullname = fullName,
                Email = register.Email,
                Password = register.Password,
                IsVolunteer = register.IsVolunteer,
                Dob = register.Dob
            };

            try
            {
                await _userRepository.InsertNewUserAsync(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new FailResponse<string>(null, $"Failed to register user: {ex.Message}"));
            }

            return Ok(new SuccessResponse<string>(null, "Registration successful"));
        }
    }
} 