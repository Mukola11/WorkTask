using Microsoft.EntityFrameworkCore;
using WorkTask.Data;
using WorkTask.Models;

namespace WorkTask.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        // Constructor
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        // Checks if a user with the given email or username exists
        public async Task<bool> UserExists(string email, string username)
        {
            return await _context.Users.AnyAsync(u => u.Email == email || u.Username == username);
        }

        // Adds a new user to the database
        public async Task CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        // Retrieves a user by email or username
        public async Task<User?> GetUserByEmailOrUsernameAsync(string usernameOrEmail)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == usernameOrEmail || u.Username == usernameOrEmail);
        }
    }
}
