using System.Security.Cryptography;

namespace KawaCzlowiekKomputer.Helpers
{
    // hashowanie haseł: PBKDF2 z losową solą, w JSON trafia tylko hash i sól
    public static class PasswordHelper
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100_000;

        // alfabet bez znaków łatwych do pomylenia (0/O, 1/l/I)
        private const string PasswordChars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789";

        // losowe hasło startowe, zawsze z cyfrą i literą
        public static string GenerateRandomPassword(int length = 10)
        {
            while (true)
            {
                var chars = new char[length];
                for (int i = 0; i < length; i++)
                    chars[i] = PasswordChars[RandomNumberGenerator.GetInt32(PasswordChars.Length)];

                string password = new(chars);
                if (password.Any(char.IsDigit) && password.Any(char.IsLetter)) return password;
            }
        }

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