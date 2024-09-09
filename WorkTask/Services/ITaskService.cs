using WorkTask.Models;
using static WorkTask.Models.SortingOptions;

namespace WorkTask.Services
{
    public interface ITaskService
    {
        Task<UserTask> CreateTaskAsync(UserTaskDto taskDto, Guid userId);
        Task<IEnumerable<UserTaskDto>> GetTasksAsync(Guid userId, TaskFilterDto filter, SortByOptions sortBy, SortOrder sortOrder);
        Task<UserTaskDto> GetTaskByIdAsync(Guid id, Guid userId);
        Task<UserTaskDto> UpdateTaskAsync(Guid id, Guid userId, UserTaskDto taskDto);
        Task<bool> DeleteTaskAsync(Guid id, Guid userId);

    }
}
