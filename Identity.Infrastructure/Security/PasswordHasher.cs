using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure.Security
{
    public static class PasswordHasher
    {
        // Dùng PasswordHasher của ASP.NET Identity, phải chỉ rõ full name
        private static readonly Microsoft.AspNetCore.Identity.PasswordHasher<string> _hasher
            = new();

        public static string Hash(string password)
        {
            return _hasher.HashPassword("user", password);
        }

        public static bool Verify(string password, string hashedPasswordFromDb)
        {
            var result = _hasher.VerifyHashedPassword("user", hashedPasswordFromDb, password);

            return result == PasswordVerificationResult.Success
                   || result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}
