using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.DTOs.User;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services
{
    /// <summary>
    /// Provides business logic for managing users, including retrieval,
    /// creation, updating, and deletion.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserService> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="UserService"/>.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public UserService(AppDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all users with optional filtering.
        /// </summary>
        /// <param name="age">Optional filter by age.</param>
        /// <param name="fullName">Optional filter by full name.</param>
        /// <param name="email">Optional filter by email.</param>
        /// <returns>A collection of user DTOs.</returns>
        public async Task<IEnumerable<UserDto>> GetUsersAsync(int? age, string? fullName, string? email)
        {
            _logger.LogDebug("Retrieving users with filters: Age={Age}, FullName={FullName}, Email={Email}", age, fullName, email);

            var query = _context.Users.AsQueryable();

            if (age.HasValue)
                query = query.Where(u => u.Age == age);

            if (!string.IsNullOrWhiteSpace(fullName))
                query = query.Where(u => u.FullName.ToLower().Contains(fullName.ToLower()));

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(u => u.Email.ToLower().Contains(email.ToLower()));

            var users = await query
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Age = u.Age,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();

            _logger.LogInformation("Retrieved {Count} users", users.Count);

            return users;
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <returns>The user DTO if found, otherwise null.</returns>
        public async Task<UserDto?> GetUserByIdAsync(Guid id)
        {
            _logger.LogDebug("Retrieving user by ID {UserId}", id);

            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Age = u.Age,
                    CreatedAt = u.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", id);
            }
            else
            {
                _logger.LogInformation("User {UserId} retrieved successfully", id);
            }

            return user;
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="request">The user creation request.</param>
        /// <returns>The created user DTO.</returns>
        public async Task<UserDto> CreateUserAsync(CreateUserRequest request)
        {
            _logger.LogInformation("Creating new user with email {Email}", request.Email);

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                Email = request.Email,
                Age = request.Age,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PasswordHash = ""
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User {UserId} created successfully", user.Id);

            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Age = user.Age,
                CreatedAt = user.CreatedAt
            };
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <param name="request">The updated user data.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        public async Task<bool> UpdateUserAsync(Guid id, UpdateUserRequest request)
        {
            _logger.LogInformation("Updating user {UserId}", id);

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning("Update failed. User {UserId} not found", id);
                return false;
            }

            user.FullName = request.FullName;
            user.Email = request.Email;
            user.Age = request.Age;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("User {UserId} updated successfully", id);
            return true;
        }

        /// <summary>
        /// Deletes an existing user by ID.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        public async Task<bool> DeleteUserAsync(Guid id)
        {
            _logger.LogInformation("Attempting to delete user {UserId}", id);

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning("Delete failed. User {UserId} not found", id);
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User {UserId} deleted successfully", id);
            return true;
        }
    }
}
