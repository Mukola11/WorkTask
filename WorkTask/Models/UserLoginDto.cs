namespace WorkTask.Models
{
    // Data transfer object for user login
    public class UserLoginDto
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }
}
