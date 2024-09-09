using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkTask.Models;
using WorkTask.Services;
using static WorkTask.Models.SortingOptions;


namespace WorkTask.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] UserTaskDto taskDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var userGuid = Guid.Parse(userId);

            var newTask = await _taskService.CreateTaskAsync(taskDto, userGuid);


            return CreatedAtAction(nameof(CreateTask), new { id = newTask.Id }, newTask);
        }


        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] TaskFilterDto filter,
                                          [FromQuery] SortByOptions sortBy = SortByOptions.DueDate,
                                          [FromQuery] SortOrder sortOrder = SortOrder.Ascending)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var tasks = await _taskService.GetTasksAsync(Guid.Parse(userId), filter, sortBy, sortOrder);

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var task = await _taskService.GetTaskByIdAsync(id, Guid.Parse(userId));

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UserTaskDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var updatedTask = await _taskService.UpdateTaskAsync(id, Guid.Parse(userId), taskDto);

            if (updatedTask == null)
            {
                return NotFound("Task not found or not authorized to update.");
            }

            return Ok(updatedTask);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var result = await _taskService.DeleteTaskAsync(id, Guid.Parse(userId));

            if (!result)
            {
                return NotFound("Task not found or not authorized to delete.");
            }

            return NoContent();
        }

    }

}
