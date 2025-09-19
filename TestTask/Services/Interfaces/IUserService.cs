using TestTask.DTOs.User;

namespace TestTask.Services.Interfaces
{
    /// <summary>
    /// Defines operations for managing user entities.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retrieves users with optional filters by age, full name, or email.
        /// </summary>
        /// <param name="age">Optional age filter.</param>
        /// <param name="fullName">Optional full name filter.</param>
        /// <param name="email">Optional email filter.</param>
        /// <returns>A collection of users matching the filters.</returns>
        Task<IEnumerable<UserDto>> GetUsersAsync(int? age, string? fullName, string? email);

        /// <summary>
        /// Retrieves a user by its unique identifier.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>The user if found, otherwise <c>null</c>.</returns>
        Task<UserDto?> GetUserByIdAsync(Guid id);

        /// <summary>
        /// Creates a new user in the system.
        /// </summary>
        /// <param name="request">The user creation request DTO.</param>
        /// <returns>The created user details.</returns>
        Task<UserDto> CreateUserAsync(CreateUserRequest request);

        /// <summary>
        /// Updates an existing user’s details.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="request">The update request DTO.</param>
        /// <returns><c>true</c> if update was successful, otherwise <c>false</c>.</returns>
        Task<bool> UpdateUserAsync(Guid id, UpdateUserRequest request);

        /// <summary>
        /// Deletes a user by its unique identifier.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns><c>true</c> if deletion was successful, otherwise <c>false</c>.</returns>
        Task<bool> DeleteUserAsync(Guid id);
    }
}
