using System.Security.Cryptography;

namespace KawaCzlowiekKomputer.Helpers
{
    // hashowanie haseł: PBKDF2 z losową solą, w JSON trafia tylko hash i sól
    public static class PasswordHelper
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100_000;

        // zwraca hash i sól w base64
        public static (string Hash, string Salt) CreateHash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
            return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
        }

        // sprawdza hasło z zapisanym hashem i solą
        public static bool Verify(string password, string hash, string salt)
        {
            try
            {
                byte[] saltBytes = Convert.FromBase64String(salt);
                byte[] expected = Convert.FromBase64String(hash);
                byte[] actual = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, Iterations, HashAlgorithmName.SHA256, HashSize);

                // porównanie w stałym czasie, odporne na ataki czasowe
                return CryptographicOperations.FixedTimeEquals(actual, expected);
            }
            catch (FormatException) { return false; } // uszkodzony wpis w JSON
        }
    }
}