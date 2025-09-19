namespace TestTask.DTOs.Auth
{
    /// <summary>
    /// Represents the request data for a login operation.
    /// </summary>
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
