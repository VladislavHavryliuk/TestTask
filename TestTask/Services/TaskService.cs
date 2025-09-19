using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.DTOs.Task;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services
{
    /// <summary>
    /// Provides business logic for managing tasks, including creation, retrieval,
    /// updating, and deletion.
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TaskService> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="TaskService"/>.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public TaskService(AppDbContext context, ILogger<TaskService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new task and returns its details.
        /// </summary>
        /// <param name="dto">The task creation data.</param>
        /// <returns>The created task in read DTO format.</returns>
        public async Task<TaskReadDto> CreateAsync(TaskCreateDto dto)
        {
            _logger.LogInformation("Creating task for user {UserId} with title '{Title}'", dto.UserId, dto.Title);

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                UserId = dto.UserId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Task {TaskId} created successfully", task.Id);

            return await MapToReadDto(task.Id);
        }

        /// <summary>
        /// Retrieves a task by its ID.
        /// </summary>
        /// <param name="id">The task identifier.</param>
        /// <returns>The task details if found, otherwise null.</returns>
        public async Task<TaskReadDto?> GetByIdAsync(Guid id)
        {
            _logger.LogDebug("Retrieving task by ID {TaskId}", id);

            var task = await _context.Tasks
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                _logger.LogWarning("Task with ID {TaskId} not found", id);
                return null;
            }

            _logger.LogInformation("Task {TaskId} retrieved successfully", id);

            return new TaskReadDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                UserId = task.UserId,
                UserFullName = task.User.FullName,
                UserEmail = task.User.Email
            };
        }

        /// <summary>
        /// Retrieves all tasks, optionally filtered by the provided parameters.
        /// </summary>
        /// <param name="filter">Optional filter criteria.</param>
        /// <returns>A collection of task read DTOs.</returns>
        public async Task<IEnumerable<TaskReadDto>> GetAllAsync(TaskFilterDto? filter = null)
        {
            _logger.LogDebug("Retrieving all tasks with filters applied: {@Filter}", filter);

            var query = _context.Tasks.Include(t => t.User).AsQueryable();

            if (filter != null)
            {
                if (filter.IsCompleted.HasValue)
                    query = query.Where(t => t.IsCompleted == filter.IsCompleted);

                if (filter.UserId.HasValue)
                    query = query.Where(t => t.UserId == filter.UserId);

                if (filter.CreatedAfter.HasValue)
                    query = query.Where(t => t.CreatedAt >= filter.CreatedAfter.Value);

                if (filter.CreatedBefore.HasValue)
                    query = query.Where(t => t.CreatedAt <= filter.CreatedBefore.Value);

                if (!string.IsNullOrWhiteSpace(filter.Title))
                    query = query.Where(t => t.Title.ToLower().Contains(filter.Title.ToLower()));

                if (!string.IsNullOrWhiteSpace(filter.UserFullName))
                    query = query.Where(t => t.User.FullName.ToLower().Contains(filter.UserFullName.ToLower()));
            }

            var tasks = await query.ToListAsync();

            _logger.LogInformation("Retrieved {Count} tasks", tasks.Count);

            return tasks.Select(t => new TaskReadDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                UserId = t.UserId,
                UserFullName = t.User.FullName,
                UserEmail = t.User.Email
            });
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="id">The task identifier.</param>
        /// <param name="dto">The task update data.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        public async Task<bool> UpdateAsync(Guid id, TaskUpdateDto dto)
        {
            _logger.LogInformation("Updating task {TaskId}", id);

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                _logger.LogWarning("Update failed. Task {TaskId} not found", id);
                return false;
            }

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.IsCompleted = dto.IsCompleted;
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Task {TaskId} updated successfully", id);
            return true;
        }

        /// <summary>
        /// Deletes a task by its ID.
        /// </summary>
        /// <param name="id">The task identifier.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            _logger.LogInformation("Attempting to delete task {TaskId}", id);

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                _logger.LogWarning("Delete failed. Task {TaskId} not found", id);
                return false;
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Task {TaskId} deleted successfully", id);
            return true;
        }

        /// <summary>
        /// Maps a task entity to its read DTO by ID.
        /// </summary>
        /// <param name="id">The task identifier.</param>
        /// <returns>The task read DTO.</returns>
        private async Task<TaskReadDto> MapToReadDto(Guid id)
        {
            _logger.LogDebug("Mapping task {TaskId} to TaskReadDto", id);

            var task = await _context.Tasks
                .Include(t => t.User)
                .FirstAsync(t => t.Id == id);

            return new TaskReadDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                UserId = task.UserId,
                UserFullName = task.User.FullName,
                UserEmail = task.User.Email
            };
        }
    }
}
