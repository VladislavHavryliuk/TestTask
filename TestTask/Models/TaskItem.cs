namespace TestTask.Models
{
    /// <summary>
    /// Represents a task item assigned to a user.
    /// </summary>
    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // FK to User
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
