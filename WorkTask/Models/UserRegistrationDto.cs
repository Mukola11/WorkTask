namespace WorkTask.Models
{
    // Data transfer object for user registration
    public class UserRegistrationDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public bool IsPasswordValid()
        {
            // Checking the length
            if (Password.Length < 8)
                return false;

            // Checking for a capital letter
            if (!Password.Any(char.IsUpper))
                return false;

            // Checking for lowercase letters
            if (!Password.Any(char.IsLower))
                return false;

            // Checking for a number
            if (!Password.Any(char.IsDigit))
                return false;

            // Checking for a special character
            if (!Password.Any(ch => !char.IsLetterOrDigit(ch)))
                return false;

            return true;
        }
    }
}
