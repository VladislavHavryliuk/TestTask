namespace TestTask.DTOs.Task
{
    /// <summary>
    /// Represents filter criteria for retrieving tasks.
    /// </summary>
    public class TaskFilterDto
    {
        public bool? IsCompleted { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
        public string? Title { get; set; }
        public string? UserFullName { get; set; }
    }
}
