﻿namespace WorkTask.Models
{
    // Represents a task within the system, including its details and relationships
    public class UserTask
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? DueDate { get; set; }

        public TaskStatus Status { get; set; } = TaskStatus.InProgress;

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key to User
        public Guid UserId { get; set; }

        public User User { get; set; }
    }

    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed
    }

    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }
}
