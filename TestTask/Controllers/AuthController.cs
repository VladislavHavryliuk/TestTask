using Microsoft.AspNetCore.Mvc;
using TestTask.DTOs.Auth;
using TestTask.Services.Interfaces;

namespace TestTask.Controllers
{
    /// <summary>
    /// Provides authentication endpoints for user registration and login.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="AuthController"/>.
        /// </summary>
        /// <param name="authService">The authentication service instance.</param>
        /// <param name="logger">The logger instance.</param>
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="request">The registration request data.</param>
        /// <returns>Returns the created user or an error message.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            _logger.LogInformation("POST /api/auth/register called for Email={Email}", request.Email);

            try
            {
                var response = await _authService.RegisterAsync(request);
                _logger.LogInformation("User {Email} registered successfully", request.Email);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Registration failed for Email={Email}", request.Email);
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Authenticates an existing user and generates a JWT token.
        /// </summary>
        /// <param name="request">The login request containing email and password.</param>
        /// <returns>Returns a JWT token if authentication is successful.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("POST /api/auth/login called for Email={Email}", request.Email);

            try
            {
                var response = await _authService.LoginAsync(request);
                _logger.LogInformation("User {Email} logged in successfully", request.Email);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Login failed for Email={Email}", request.Email);
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
