using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestTask.DTOs.User;
using TestTask.Services.Interfaces;

namespace TestTask.Controllers
{
    /// <summary>
    /// Provides endpoints for managing users.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="UserController"/>.
        /// </summary>
        /// <param name="userService">The user service instance.</param>
        /// <param name="logger">The logger instance.</param>
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all users with optional filtering.
        /// </summary>
        /// <param name="age">Optional filter by age.</param>
        /// <param name="fullName">Optional filter by full name.</param>
        /// <param name="email">Optional filter by email.</param>
        /// <returns>Returns a list of users matching the filters.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers(
            [FromQuery] int? age,
            [FromQuery] string? fullName,
            [FromQuery] string? email)
        {
            _logger.LogInformation("GET /api/user called with filters Age={Age}, FullName={FullName}, Email={Email}", age, fullName, email);

            var users = await _userService.GetUsersAsync(age, fullName, email);

            _logger.LogInformation("{Count} users retrieved", users.Count());

            return Ok(users);
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <returns>Returns the user details if found.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            _logger.LogInformation("GET /api/user/{Id} called", id);

            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                _logger.LogWarning("User {Id} not found", id);
                return NotFound();
            }

            _logger.LogInformation("User {Id} retrieved successfully", id);
            return Ok(user);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="request">The user creation request.</param>
        /// <returns>Returns the created user details.</returns>
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserRequest request)
        {
            _logger.LogInformation("POST /api/user called for Email={Email}", request.Email);

            var user = await _userService.CreateUserAsync(request);

            _logger.LogInformation("User {Id} created successfully", user.Id);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <param name="request">The updated user data.</param>
        /// <returns>Returns 204 No Content if successful, otherwise 404.</returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
        {
            _logger.LogInformation("PUT /api/user/{Id} called", id);

            var updated = await _userService.UpdateUserAsync(id, request);

            if (!updated)
            {
                _logger.LogWarning("User {Id} update failed - not found", id);
                return NotFound();
            }

            _logger.LogInformation("User {Id} updated successfully", id);
            return NoContent();
        }

        /// <summary>
        /// Deletes an existing user.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <returns>Returns 204 No Content if successful, otherwise 404.</returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            _logger.LogInformation("DELETE /api/user/{Id} called", id);

            var deleted = await _userService.DeleteUserAsync(id);

            if (!deleted)
            {
                _logger.LogWarning("User {Id} deletion failed - not found", id);
                return NotFound();
            }

            _logger.LogInformation("User {Id} deleted successfully", id);
            return NoContent();
        }
    }
}
