namespace TestTask.DTOs.Auth
{
    /// <summary>
    /// Represents the response returned after a successful authentication operation.
    /// </summary>
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}
