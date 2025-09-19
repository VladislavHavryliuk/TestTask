using TestTask.DTOs.Task;

namespace TestTask.Services.Interfaces
{
    /// <summary>
    /// Defines operations for managing task entities.
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Creates a new task for a specific user.
        /// </summary>
        /// <param name="dto">The task creation DTO.</param>
        /// <returns>The created task with additional details.</returns>
        Task<TaskReadDto> CreateAsync(TaskCreateDto dto);

        /// <summary>
        /// Retrieves a task by its unique identifier.
        /// </summary>
        /// <param name="id">The task ID.</param>
        /// <returns>The task if found, otherwise <c>null</c>.</returns>
        Task<TaskReadDto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Retrieves all tasks, optionally filtered by completion status, user, or creation date.
        /// </summary>
        /// <param name="filter">Optional filtering parameters.</param>
        /// <returns>A collection of tasks.</returns>
        Task<IEnumerable<TaskReadDto>> GetAllAsync(TaskFilterDto? filter = null);

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="id">The task ID.</param>
        /// <param name="dto">The update DTO containing new values.</param>
        /// <returns><c>true</c> if update was successful, otherwise <c>false</c>.</returns>
        Task<bool> UpdateAsync(Guid id, TaskUpdateDto dto);

        /// <summary>
        /// Deletes a task by its unique identifier.
        /// </summary>
        /// <param name="id">The task ID.</param>
        /// <returns><c>true</c> if deletion was successful, otherwise <c>false</c>.</returns>
        Task<bool> DeleteAsync(Guid id);
    }
}
