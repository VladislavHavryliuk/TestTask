using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestTask.DTOs.Task;
using TestTask.Services.Interfaces;

namespace TestTask.Controllers
{
    /// <summary>
    /// Provides endpoints to manage tasks for authenticated users.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskController> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="TaskController"/>.
        /// </summary>
        /// <param name="taskService">The task service instance.</param>
        /// <param name="logger">The logger instance.</param>
        public TaskController(ITaskService taskService, ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new task for the current user.
        /// </summary>
        /// <param name="dto">The task creation data.</param>
        /// <returns>Returns the created task details.</returns>
        [HttpPost]
        public async Task<ActionResult<TaskReadDto>> Create([FromBody] TaskCreateDto dto)
        {
            _logger.LogInformation("POST /api/task called for UserId={UserId}", dto.UserId);

            var task = await _taskService.CreateAsync(dto);

            _logger.LogInformation("Task {TaskId} created successfully for UserId={UserId}", task.Id, dto.UserId);

            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        /// <summary>
        /// Retrieves a task by its unique identifier.
        /// </summary>
        /// <param name="id">The task identifier.</param>
        /// <returns>Returns the task details if found.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TaskReadDto>> GetById(Guid id)
        {
            _logger.LogInformation("GET /api/task/{Id} called", id);

            var task = await _taskService.GetByIdAsync(id);

            if (task == null)
            {
                _logger.LogWarning("Task {Id} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Task {Id} retrieved successfully", id);
            return Ok(task);
        }

        /// <summary>
        /// Retrieves all tasks for the current user with optional filtering.
        /// </summary>
        /// <param name="filter">Filter parameters such as completion status.</param>
        /// <returns>Returns a list of tasks matching the filter.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskReadDto>>> GetAll([FromQuery] TaskFilterDto filter)
        {
            _logger.LogInformation("GET /api/task called with filter: {@Filter}", filter);

            var tasks = await _taskService.GetAllAsync(filter);

            _logger.LogInformation("{Count} tasks retrieved", tasks.Count());

            return Ok(tasks);
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="id">The task identifier.</param>
        /// <param name="dto">The updated task data.</param>
        /// <returns>Returns 204 No Content if successful, otherwise 404.</returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TaskUpdateDto dto)
        {
            _logger.LogInformation("PUT /api/task/{Id} called", id);

            var updated = await _taskService.UpdateAsync(id, dto);

            if (!updated)
            {
                _logger.LogWarning("Task {Id} update failed - not found", id);
                return NotFound();
            }

            _logger.LogInformation("Task {Id} updated successfully", id);
            return NoContent();
        }

        /// <summary>
        /// Deletes an existing task.
        /// </summary>
        /// <param name="id">The task identifier.</param>
        /// <returns>Returns 204 No Content if successful, otherwise 404.</returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("DELETE /api/task/{Id} called", id);

            var deleted = await _taskService.DeleteAsync(id);

            if (!deleted)
            {
                _logger.LogWarning("Task {Id} deletion failed - not found", id);
                return NotFound();
            }

            _logger.LogInformation("Task {Id} deleted successfully", id);
            return NoContent();
        }
    }
}
