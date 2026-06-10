using BC = BCrypt.Net.BCrypt;

namespace eVote360_Pro.Core.Application.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            return BC.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BC.Verify(password, hashedPassword);
        }
    }
}