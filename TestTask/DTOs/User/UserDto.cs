namespace TestTask.DTOs.User
{
    /// <summary>
    /// Represents the data returned when retrieving a user.
    /// </summary>
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
