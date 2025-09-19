using System.Collections.Generic;


namespace TestTask.Models
{
    /// <summary>
    /// Represents an application user who can own multiple tasks.
    /// </summary>
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public int Age { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // One to Many
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
