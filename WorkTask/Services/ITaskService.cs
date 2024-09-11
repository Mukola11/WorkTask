using WorkTask.Models;
using static WorkTask.Models.SortingOptions;

namespace WorkTask.Services
{
    public interface ITaskService
    {
        // Creates a new task and associates it with the specified user
        Task<UserTask> CreateTaskAsync(UserTaskDto taskDto, Guid userId);

        // Retrieves a list of tasks for a user, applying filters and sorting options
        Task<IEnumerable<UserTaskDto>> GetTasksAsync(Guid userId, TaskFilterDto filter, SortByOptions sortBy, SortOrder sortOrder);

        // Retrieves a single task by its ID, ensuring it belongs to the specified user
        Task<UserTaskDto?> GetTaskByIdAsync(Guid id, Guid userId);

        // Updates an existing task and returns the updated task data
        Task<UserTaskDto?> UpdateTaskAsync(Guid id, Guid userId, UserTaskDto taskDto);

        // Deletes a task by its ID, ensuring it belongs to the specified user
        Task<bool> DeleteTaskAsync(Guid id, Guid userId);

    }
}
