namespace TestTask.DTOs.Task
{
    /// <summary>
    /// Represents the data required to update an existing task.
    /// </summary>
    public class TaskUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
