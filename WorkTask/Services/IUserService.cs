using WorkTask.Models;

namespace WorkTask.Services
{
    public interface IUserService
    {
        Task<bool> UserExists(string email, string username);
        Task CreateUserAsync(User user);
        Task<User> GetUserByEmailOrUsernameAsync(string usernameOrEmail);
    }

}
