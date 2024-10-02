using System;
using System.Text;
using Konscious.Security.Cryptography;
using BCrypt.Net;

namespace ATM_APP
{
   public  class Hashing_Service
    {
        // Argon2id Hashing
        public static string Argon2id_HashPassword(string password)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));
            argon2.Salt = GenerateSalt(16); // Генерація випадкової солі
            argon2.DegreeOfParallelism = 4; // Кількість потоків
            argon2.Iterations = 4; // Кількість ітерацій
            argon2.MemorySize = 1024 * 1024; // Розмір пам'яті

            // Хешуємо пароль
            byte[] hash = argon2.GetBytes(32); // Вивід хешу розміром 32 байти

            return Convert.ToBase64String(argon2.Salt) + "$" + Convert.ToBase64String(hash);
        }

        public static bool Argon2id_VerifyPassword(string password, string hashedPassword)
        {
            var parts = hashedPassword.Split('$');
            if (parts.Length != 2) return false;

            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] expectedHash = Convert.FromBase64String(parts[1]);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            using (var argon2 = new Argon2id(passwordBytes))
            {
                argon2.Salt = salt;
                argon2.DegreeOfParallelism = 4;
                argon2.Iterations = 4;
                argon2.MemorySize = 1024 * 1024;

                byte[] hash = argon2.GetBytes(32);

                for (int i = 0; i < hash.Length; i++)
                {
                    if (hash[i] != expectedHash[i])
                        return false;
                }
                return true;
            }
        }

        // BCrypt Hashing
        public static string BCrypt_HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool BCryptVerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        // Генерація випадкової солі
        private static byte[] GenerateSalt(int size)
        {
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            byte[] salt = new byte[size];
            rng.GetBytes(salt);
            return salt;
        }
    }
}
