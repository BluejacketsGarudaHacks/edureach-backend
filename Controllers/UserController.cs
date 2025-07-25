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
        private readonly ImageUtil _imageUtil;
        
        public UserController(UserRepository UserRepository, JwtUtil jwtUtil, ImageUtil imageUtil)
        {
            _userRepository = UserRepository;
            _jwtUtil = jwtUtil;
            _imageUtil = imageUtil;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var userId = await _userRepository.GetUserIdByEmailAndPasswordAsync(login.Email, login.Password);
            if (userId == null)
            {
                return NotFound(new FailResponse<string>(null, "Kredensial salah."));
            }

            var expires = DateTime.UtcNow.AddDays(7);
            var jwtToken = _jwtUtil.GenerateToken(userId.ToString(), expires);
            return Ok(new SuccessResponse<string>(jwtToken, "Berhasil login"));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest register)
        {
            var errors = RegisterValidator.Validate(register);
            if (errors.Any())
            {
                return BadRequest(new FailResponse<List<string>>(errors, "Validasi gagal"));
            }
            
            var user = await _userRepository.GetUserByEmail(register.Email);
            if(user != null) {
                return BadRequest(new FailResponse<string>(null, "Email sudah terdaftar"));
            }

            var fullName = string.Concat(register.FirstName, " ", register.LastName);
            user = new User 
            {
                Id = Guid.NewGuid(),
                FullName = fullName,
                Email = register.Email,
                Password = register.Password,
                IsVolunteer = register.IsVolunteer,
                Dob = register.Dob,
                ImagePath = "/Images/basic_profile.png"
            };

            try
            {
                await _userRepository.InsertNewUserAsync(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new FailResponse<string>(null, $"Gagal mendaftarkan user: {ex.Message}"));
            }

            return Ok(new SuccessResponse<string>(null, "Pendaftaran berhasil"));
        }
        
        
        [HttpPut]
        public async Task<ActionResult<User>> 
            UpdateUser([FromForm] UpdateUserRequest update)
        {
            var userId = Guid.Parse(HttpContext.Items["UserId"]!.ToString()!);
            var errors = UpdateUserValidator.Validate(update);

            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return NotFound(new FailResponse<string>(null, "User not found"));
            }

            var FullName = string.Concat(update.FirstName, " ", update.LastName);

            user.FullName = FullName;
            user.Email = update.Email;
            user.IsVolunteer = update.IsVolunteer;
            user.Dob = update.Dob;
           
            if (update.Image != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await update.Image.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();
                    var imageName = _imageUtil.SaveImage(imageBytes, update.Image.FileName);
                    user.ImagePath = imageName;
                }
            }
            
            var result = await _userRepository.UpdateUser(userId, user);

            return result;
        }

        [HttpPut("password")]
        public async Task<ActionResult<User>> UpdatePassword([FromBody] UpdatePasswordDto dto)
        {
            var userId = Guid.Parse(HttpContext.Items["UserId"]!.ToString()!);
            var errors = PasswordValidator.Validate(dto.Password, dto.ConfirmPassword);
            if (errors.Any())
            {
                return BadRequest(new FailResponse<List<string>>(errors, "Validasi gagal"));
            }

            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return NotFound(new FailResponse<string>(null, "User not found"));
            }

            user.Password = dto.Password;
            var result = await _userRepository.UpdateUser(userId, user);

            return result;
        }

        [HttpGet]
        public async Task<ActionResult<User>> Get()
        {
            var userId = Guid.Parse(HttpContext.Items["UserId"]!.ToString()!);
            var user = await _userRepository.GetUserById(userId);

            if(user == null) {
                return NotFound("User not found");
            }

            var userResponse = new UserResponseDto
            {
                Id = userId,
                FullName = user.FullName,
                Email = user.Email,
                IsVolunteer = user.IsVolunteer,
                Dob = user.Dob,
                ImagePath = user.ImagePath
            };

            return Ok(userResponse);
        }


        [HttpPost("add-notification")]
        public async Task<ActionResult<Notification>> AddNotification([FromBody] NotificationRequest notificationRequest)
        {
            var userId = Guid.Parse(HttpContext.Items["UserId"]!.ToString()!);
            var notification = new Notification()
            {
                UserId = userId,
                Message = notificationRequest.Message,
                IsShown = false,
                IsChecked = false,
            };
            await _userRepository.AddNotificationAsync(notification);

            return Ok(notification);
        }

        [HttpPut("update-notification/{id}")]
        public async Task<ActionResult<Notification>>UpdateNotification(Guid id, [FromBody] NotificationRequest notificationRequest)
        {
            var userId = Guid.Parse(HttpContext.Items["UserId"]!.ToString()!);
            var notification = new Notification()
            {
                UserId = userId,
                Message = notificationRequest.Message,
                IsShown = notificationRequest.IsShown,
                IsChecked = notificationRequest.IsChecked,
            };

            notification = await _userRepository.UpdateNotificationAsync(id, notification);
            return Ok(notification);
        }

        [HttpGet("notification/user")]
        public async Task<ActionResult<ICollection<Notification>>>GetUserNotification()
        {
            var userId = Guid.Parse(HttpContext.Items["UserId"]!.ToString()!);
            var notifications = await _userRepository.GetUserNotificationsAsync(userId);

            return Ok(notifications);
        }

    }
} 