namespace WorkTask.Models
{
    // Data Transfer Object (DTO) representing a task for communication between layers
    public class UserTaskDto
    {
        public Guid Id { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskStatus Status { get; set; } 
        public TaskPriority Priority { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
