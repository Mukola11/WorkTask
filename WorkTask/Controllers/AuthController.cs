using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorkTask.Data;
using WorkTask.Models;
using WorkTask.Services;

namespace WorkTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        // Constructor 
        public AuthController(IUserService userService, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _userService = userService;
            _configuration = configuration;
            _logger = logger;
        }

        // Endpoint for user registration
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDto request)
        {
            _logger.LogInformation("Registering user with email {Email} and username {Username}", request.Email, request.Username);

            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for user registration");
                return BadRequest(ModelState);
            }

            // Check if user already exists
            if (await _userService.UserExists(request.Email, request.Username))
            {
                _logger.LogWarning("User with email {Email} or username {Username} already exists", request.Email, request.Username);
                return BadRequest("User with this email or username already exists.");
            }

            // Validate password complexity
            if (!request.IsPasswordValid())
            {
                _logger.LogWarning("Password does not meet complexity requirements for user {Username}", request.Username);
                return BadRequest("Password does not meet complexity requirements.");
            }

            // Create new user
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await _userService.CreateUserAsync(user);
            _logger.LogInformation("User {Username} registered successfully", request.Username);

            return Ok("User registered successfully.");
        }

        // Endpoint for user login
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            _logger.LogInformation("User login attempt with email or username {UsernameOrEmail}", request.UsernameOrEmail);

            // Retrieve user by email or username
            var user = await _userService.GetUserByEmailOrUsernameAsync(request.UsernameOrEmail);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                _logger.LogWarning("Invalid login attempt for {UsernameOrEmail}", request.UsernameOrEmail);
                return Unauthorized("Invalid credentials.");
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);
            _logger.LogInformation("User {Username} logged in successfully", user.Username);

            return Ok(new { token });
        }

        // Generate JWT token for the authenticated user
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
