using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkTask.Models;
using WorkTask.Services;
using static WorkTask.Models.SortingOptions;


namespace WorkTask.Controllers
{
    // Controller for managing tasks, requires authorization
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskController> _logger;

        // Constructor
        public TaskController(ITaskService taskService, ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        // Endpoint for creating a new task
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] UserTaskDto taskDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                _logger.LogWarning("User ID is null while creating task.");
                return Unauthorized();
            }

            var userGuid = Guid.Parse(userId);

            _logger.LogInformation("Creating task for user {UserId}", userId);
            var newTask = await _taskService.CreateTaskAsync(taskDto, userGuid);

            _logger.LogInformation("Task created successfully with ID {TaskId}", newTask.Id);

            return CreatedAtAction(nameof(CreateTask), new { id = newTask.Id }, newTask);
        }

        // Endpoint for retrieving tasks with optional filters and sorting
        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] TaskFilterDto filter,
                                                  [FromQuery] SortByOptions sortBy = SortByOptions.DueDate,
                                                  [FromQuery] SortOrder sortOrder = SortOrder.Ascending)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                _logger.LogWarning("User ID is null while fetching tasks.");
                return Unauthorized();
            }

            _logger.LogInformation("Fetching tasks for user {UserId} with filter {Filter}", userId, filter);
            var tasks = await _taskService.GetTasksAsync(Guid.Parse(userId), filter, sortBy, sortOrder);

            _logger.LogInformation("Fetched {TaskCount} tasks for user {UserId}", tasks.Count(), userId);

            return Ok(tasks);
        }

        // Endpoint for retrieving a specific task by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                _logger.LogWarning("User ID is null while fetching task with ID {TaskId}", id);
                return Unauthorized();
            }

            _logger.LogInformation("Fetching task with ID {TaskId} for user {UserId}", id, userId);
            var task = await _taskService.GetTaskByIdAsync(id, Guid.Parse(userId));

            if (task == null)
            {
                _logger.LogWarning("Task with ID {TaskId} not found or not authorized for user {UserId}", id, userId);
                return NotFound();
            }

            _logger.LogInformation("Task with ID {TaskId} fetched successfully for user {UserId}", id, userId);

            return Ok(task);
        }

        // Endpoint for updating a specific task by ID
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UserTaskDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state while updating task with ID {TaskId}", id);
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                _logger.LogWarning("User ID is null while updating task with ID {TaskId}", id);
                return Unauthorized();
            }

            _logger.LogInformation("Updating task with ID {TaskId} for user {UserId}", id, userId);
            var updatedTask = await _taskService.UpdateTaskAsync(id, Guid.Parse(userId), taskDto);

            if (updatedTask == null)
            {
                _logger.LogWarning("Task with ID {TaskId} not found or not authorized for user {UserId}", id, userId);
                return NotFound();
            }

            _logger.LogInformation("Task with ID {TaskId} updated successfully for user {UserId}", id, userId);

            return Ok(updatedTask);
        }

        // Endpoint for deleting a specific task by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                _logger.LogWarning("User ID is null while deleting task with ID {TaskId}", id);
                return Unauthorized();
            }

            _logger.LogInformation("Deleting task with ID {TaskId} for user {UserId}", id, userId);
            var success = await _taskService.DeleteTaskAsync(id, Guid.Parse(userId));

            if (!success)
            {
                _logger.LogWarning("Task with ID {TaskId} not found or not authorized for user {UserId}", id, userId);
                return NotFound();
            }

            _logger.LogInformation("Task with ID {TaskId} deleted successfully for user {UserId}", id, userId);

            return NoContent();
        }
    }
}
