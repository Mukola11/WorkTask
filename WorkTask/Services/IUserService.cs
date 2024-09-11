using WorkTask.Models;

namespace WorkTask.Services
{
    public interface IUserService
    {
        // Checks if a user with the given email or username already exists
        Task<bool> UserExists(string email, string username);

        // Creates a new user and saves it to the database
        Task CreateUserAsync(User user);

        // Retrieves a user by their email or username
        Task<User?> GetUserByEmailOrUsernameAsync(string usernameOrEmail);
    }

}
