using System.ComponentModel.DataAnnotations;

namespace TestTask.DTOs.User
{
    /// <summary>
    /// Represents the request data for creating a new user.
    /// </summary>
    public class CreateUserRequest
    {
        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, Range(1, 120)]
        public int Age { get; set; }
    }
}