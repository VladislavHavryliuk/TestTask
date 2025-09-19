namespace TestTask.DTOs.Auth
{
    /// <summary>
    /// Represents the request data for a user registration operation.
    /// </summary>
    public class RegisterRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Age { get; set; }
    }
}
