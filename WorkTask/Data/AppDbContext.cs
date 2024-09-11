using Microsoft.EntityFrameworkCore;
using WorkTask.Models;

namespace WorkTask.Data
{
    // Represents the application's database context
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSet for accessing User entities in the database
        public DbSet<User> Users { get; set; }

        // DbSet for accessing UserTask entities in the database
        public DbSet<UserTask> UserTasks { get; set; }
    }
}

