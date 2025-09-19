using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestTask.Data;
using TestTask.DTOs.Auth;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services
{
    /// <summary>
    /// Provides authentication-related business logic including user registration,
    /// login, and JWT token generation.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="AuthService"/>.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="logger">The logger instance.</param>
        public AuthService(AppDbContext context, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user and issues a JWT token.
        /// </summary>
        /// <param name="request">The registration request containing user details and password.</param>
        /// <returns>Returns an authentication response containing the token and expiration.</returns>
        /// <exception cref="Exception">Thrown if the email is already registered.</exception>
        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            _logger.LogInformation("Attempting to register user with email: {Email}", request.Email);

            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                _logger.LogWarning("Registration failed. User with email {Email} already exists.", request.Email);
                throw new Exception("User with this email already exists.");
            }

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                Age = request.Age,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User {Email} registered successfully with ID {UserId}", user.Email, user.Id);

            return GenerateJwtToken(user);
        }

        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// </summary>
        /// <param name="request">The login request containing email and password.</param>
        /// <returns>Returns an authentication response containing the token and expiration.</returns>
        /// <exception cref="Exception">Thrown if the credentials are invalid.</exception>
        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            _logger.LogInformation("Login attempt for email: {Email}", request.Email);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                _logger.LogWarning("Invalid login attempt for email: {Email}", request.Email);
                throw new Exception("Invalid email or password.");
            }

            _logger.LogInformation("User {Email} logged in successfully", user.Email);

            return GenerateJwtToken(user);
        }

        /// <summary>
        /// Generates a JWT token for the given user.
        /// </summary>
        /// <param name="user">The authenticated user.</param>
        /// <returns>An authentication response with token and expiration details.</returns>
        private AuthResponse GenerateJwtToken(User user)
        {
            _logger.LogDebug("Generating JWT token for user {Email}", user.Email);

            var jwtSettings = _configuration.GetSection("JwtSettings");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"]!));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("FullName", user.FullName)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            _logger.LogDebug("JWT token generated successfully for user {Email}", user.Email);

            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expires
            };
        }
    }
}
