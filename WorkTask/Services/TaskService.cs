using Microsoft.EntityFrameworkCore;
using WorkTask.Data;
using WorkTask.Models;
using static WorkTask.Models.SortingOptions;


namespace WorkTask.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserTask> CreateTaskAsync(UserTaskDto taskDto, Guid userId)
        {
            var newTask = new UserTask
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                DueDate = taskDto.DueDate,
                Status = taskDto.Status,
                Priority = taskDto.Priority,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.UserTasks.Add(newTask);
            await _context.SaveChangesAsync();

            return newTask;
        }

        public async Task<IEnumerable<UserTaskDto>> GetTasksAsync(Guid userId, TaskFilterDto filter, SortByOptions sortBy, SortOrder sortOrder)
        {
            var query = _context.UserTasks.AsQueryable();

            query = query.Where(t => t.UserId == userId);


            if (filter.Status.HasValue)
            {
                query = query.Where(t => t.Status == filter.Status.Value);
            }

            if (filter.DueDate.HasValue)
            {
                var dueDate = filter.DueDate.Value.Date;
                query = query.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == dueDate);
            }

            if (filter.Priority.HasValue)
            {
                query = query.Where(t => t.Priority == filter.Priority.Value);
            }


            switch (sortBy)
            {
                case SortByOptions.DueDate:
                    query = sortOrder == SortOrder.Ascending ? query.OrderBy(t => t.DueDate) : query.OrderByDescending(t => t.DueDate);
                    break;
                case SortByOptions.Priority:
                    query = sortOrder == SortOrder.Ascending ? query.OrderBy(t => t.Priority) : query.OrderByDescending(t => t.Priority);
                    break;
            }


            var skip = (filter.Page - 1) * filter.PageSize;
            query = query.Skip(skip).Take(filter.PageSize);

            var tasks = await query.ToListAsync();
            return tasks.Select(task => new UserTaskDto
            {
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                Priority = task.Priority
            });

        }

        public async Task<UserTaskDto> GetTaskByIdAsync(Guid id, Guid userId)
        {
            var task = await _context.UserTasks
                .Where(t => t.Id == id && t.UserId == userId)
                .FirstOrDefaultAsync();

            if (task == null)
            {
                return null;
            }

            return new UserTaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                Priority = task.Priority,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
        }

        public async Task<UserTaskDto> UpdateTaskAsync(Guid id, Guid userId, UserTaskDto taskDto)
        {
            var task = await _context.UserTasks
                .Where(t => t.Id == id && t.UserId == userId)
                .FirstOrDefaultAsync();

            if (task == null)
            {
                return null; 
            }


            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.DueDate = taskDto.DueDate;
            task.Status = taskDto.Status;
            task.Priority = taskDto.Priority;
            task.UpdatedAt = DateTime.UtcNow;

            _context.UserTasks.Update(task);
            await _context.SaveChangesAsync();

            return new UserTaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                Priority = task.Priority,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
        }

        public async Task<bool> DeleteTaskAsync(Guid id, Guid userId)
        {
            var task = await _context.UserTasks
                .Where(t => t.Id == id && t.UserId == userId)
                .FirstOrDefaultAsync();

            if (task == null)
            {
                return false;
            }

            _context.UserTasks.Remove(task);
            await _context.SaveChangesAsync();

            return true; 
        }

    }

}
