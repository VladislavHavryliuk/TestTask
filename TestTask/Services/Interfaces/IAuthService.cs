using TestTask.DTOs.Auth;

namespace TestTask.Services.Interfaces
{
    /// <summary>
    /// Defines authentication-related operations such as user registration and login.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user and returns an authentication response with a JWT token.
        /// </summary>
        /// <param name="request">The registration request containing user details.</param>
        /// <returns>An <see cref="AuthResponse"/> with token and expiration details.</returns>
        Task<AuthResponse> RegisterAsync(RegisterRequest request);

        /// <summary>
        /// Authenticates a user and returns a JWT token if the credentials are valid.
        /// </summary>
        /// <param name="request">The login request containing email and password.</param>
        /// <returns>An <see cref="AuthResponse"/> with token and expiration details.</returns>
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}
