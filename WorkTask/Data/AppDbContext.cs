using Microsoft.EntityFrameworkCore;
using WorkTask.Models;

namespace WorkTask.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<User> Users { get; set; }

        public DbSet<UserTask> UserTasks { get; set; }
    }
}

