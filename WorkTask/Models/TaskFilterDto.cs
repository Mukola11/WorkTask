namespace WorkTask.Models
{
    // Data transfer object for filtering tasks
    public class TaskFilterDto
    {
        // Optional filter by task status (Pending, InProgress, Completed)
        public TaskStatus? Status { get; set; }

        // Optional filter by due date of the task
        public DateTime? DueDate { get; set; }

        // Optional filter by task priority (Low, Medium, High)
        public TaskPriority? Priority { get; set; }

        // Page number for pagination, default is 1
        public int Page { get; set; } = 1;

        // Number of tasks per page for pagination, default is 10
        public int PageSize { get; set; } = 10;
    }

}
