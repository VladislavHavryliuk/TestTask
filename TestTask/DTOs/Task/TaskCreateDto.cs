namespace TestTask.DTOs.Task
{
    /// <summary>
    /// Represents the data required to create a new task.
    /// </summary>
    public class TaskCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid UserId { get; set; }
    }
}
