using System;
using System.Text;
using Konscious.Security.Cryptography;
using BCrypt.Net;

namespace ATM_Winforms
{
    internal class Hashing_Service
    {
        public static string Argon2id_HashPassword(string password)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));
            argon2.Salt = Encoding.UTF8.GetBytes("someSalt"); // Сіль
            argon2.DegreeOfParallelism = 4; // Кількість потоків
            argon2.Iterations = 4; // Кількість ітерацій
            argon2.MemorySize = 1024 * 1024; // Розмір пам'яті

            // Хешуємо пароль
            byte[] hash = argon2.GetBytes(32); // Вивід хешу розміром 32 байти

            return Convert.ToBase64String(hash);
        }

        public static bool Argon2id_VerifyPassword(string password, string hashedPassword)
        {
            byte[] expectedHash = Convert.FromBase64String(hashedPassword);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            using (var argon2 = new Argon2id(passwordBytes))
            {
                argon2.Salt = Encoding.UTF8.GetBytes("someSalt"); // Сіль
                argon2.DegreeOfParallelism = 4; // Кількість потоків
                argon2.Iterations = 4; // Кількість ітерацій
                argon2.MemorySize = 1024 * 1024; // Розмір пам'яті

                // Хешуємо пароль для порівняння з очікуваним хешем
                byte[] hash = argon2.GetBytes(32); // Вивід хешу розміром 32 байти

                // Порівнюємо хеші
                for (int i = 0; i < hash.Length; i++)
                {
                    if (hash[i] != expectedHash[i])
                        return false;
                }
                return true;
            }
        }

        public static string BCrypt_HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool BCryptVerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public static string SHA_HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
