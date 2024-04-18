using ATM_Winforms;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace ATM_Winforms
{
    internal class Encryption_Manager
    {

        // Функція для зашифрування даних гібридним алгоритмом
        public static byte[] EncryptData(byte[] data, RSAParameters publicKey)
        {
            // Генеруємо симетричний ключ і вектор ініціалізації для AES
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.GenerateKey();
                aesAlg.GenerateIV();

                // Шифруємо дані AES ключем
                byte[] encryptedData;
                using (var encryptor = aesAlg.CreateEncryptor())
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(data, 0, data.Length);
                    }
                    encryptedData = msEncrypt.ToArray();
                }

                // Шифруємо AES ключ RSA публічним ключем
                byte[] encryptedKey;
                using (var rsa = RSA.Create())
                {
                    rsa.ImportParameters(publicKey);
                    encryptedKey = rsa.Encrypt(aesAlg.Key, RSAEncryptionPadding.OaepSHA256);
                }

                // Об'єднуємо зашифрований AES ключ, зашифровані дані та вектор ініціалізації в один масив
                byte[] result = new byte[encryptedKey.Length + aesAlg.IV.Length + encryptedData.Length];
                Buffer.BlockCopy(encryptedKey, 0, result, 0, encryptedKey.Length);
                Buffer.BlockCopy(aesAlg.IV, 0, result, encryptedKey.Length, aesAlg.IV.Length);
                Buffer.BlockCopy(encryptedData, 0, result, encryptedKey.Length + aesAlg.IV.Length, encryptedData.Length);
                return result;
            }
        }

        // Функція для розшифрування даних гібридним алгоритмом
        public static byte[] DecryptData(byte[] encryptedData, RSAParameters privateKey)
        {
            using (Aes aesAlg = Aes.Create())
            {
                // Витягуємо зашифрований AES ключ, вектор ініціалізації та дані з зашифрованих даних
                byte[] encryptedKey = new byte[256];
                byte[] iv = new byte[16];
                byte[] data = new byte[encryptedData.Length - 256 - 16];
                Buffer.BlockCopy(encryptedData, 0, encryptedKey, 0, 256);
                Buffer.BlockCopy(encryptedData, 256, iv, 0, 16);
                Buffer.BlockCopy(encryptedData, 256 + 16, data, 0, data.Length);

                // Розшифровуємо AES ключ RSA приватним ключем
                byte[] decryptedKey;
                using (var rsa = RSA.Create())
                {
                    rsa.ImportParameters(privateKey);
                    decryptedKey = rsa.Decrypt(encryptedKey, RSAEncryptionPadding.OaepSHA256);
                }

                // Розшифровуємо дані AES ключем та вектором ініціалізації
                using (var decryptor = aesAlg.CreateDecryptor(decryptedKey, iv))
                using (var msDecrypt = new MemoryStream(data))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var decryptedData = new MemoryStream())
                {
                    csDecrypt.CopyTo(decryptedData);
                    return decryptedData.ToArray();
                }
            }
        }

        // Протокол Нідгема — Шредера для обміну ключами
        public static (byte[] senderEncryptedKey, byte[] receiverEncryptedKey) NiedebergShraderProtocol(RSAParameters senderPublicKey, RSAParameters receiverPublicKey)
        {
            // Генеруємо випадковий симетричний ключ для AES
            byte[] aesKey;
            using (Aes aes = Aes.Create())
            {
                aesKey = aes.Key;
            }

            // Шифруємо симетричний ключ двома різними RSA публічними ключами
            using (RSA rsaSender = RSA.Create())
            using (RSA rsaReceiver = RSA.Create())
            {
                rsaSender.ImportParameters(senderPublicKey);
                rsaReceiver.ImportParameters(receiverPublicKey);

                byte[] senderEncryptedKey = rsaSender.Encrypt(aesKey, RSAEncryptionPadding.OaepSHA256);
                byte[] receiverEncryptedKey = rsaReceiver.Encrypt(aesKey, RSAEncryptionPadding.OaepSHA256);

                return (senderEncryptedKey, receiverEncryptedKey);
            }
        }


        public void Example()
        {
            // Генеруємо RSA ключі для обох сторін
            var rsaAlice = RSA.Create();
            var rsaBob = RSA.Create();
            var alicePrivateKey = rsaAlice.ExportParameters(true);
            var alicePublicKey = rsaAlice.ExportParameters(false);
            var bobPrivateKey = rsaBob.ExportParameters(true);
            var bobPublicKey = rsaBob.ExportParameters(false);


            // Виконуємо Протокол Нідгема — Шредера для обміну ключами
            (byte[] aliceEncryptedKey, byte[] bobEncryptedKey) = NiedebergShraderProtocol(alicePublicKey, bobPublicKey);
            // Виводимо aliceEncryptedKey та bobEncryptedKey
            Console.WriteLine("Alice's Encrypted Key:");
            Console.WriteLine(BitConverter.ToString(aliceEncryptedKey).Replace("-", ""));

            Console.WriteLine("\nBob's Encrypted Key:");
            Console.WriteLine(BitConverter.ToString(bobEncryptedKey).Replace("-", ""));
            // Приклад використання шифрування та розшифрування даних
            string originalData = "Hello, World!";
            byte[] encryptedData = EncryptData(Encoding.UTF8.GetBytes(originalData), alicePublicKey);
            Console.WriteLine(Encoding.UTF8.GetString(encryptedData));


            // Розшифровуємо дані
            byte[] decryptedData = DecryptData(encryptedData, alicePrivateKey);
            Console.WriteLine(Encoding.UTF8.GetString(decryptedData));


            // Виводимо публічний ключ Alice у форматі SubjectPublicKeyInfo
            Console.WriteLine("Alice's Public Key:");
            var alicePublicKeyBytes = rsaAlice.ExportSubjectPublicKeyInfo();
            var alicePublicKeyString = BitConverter.ToString(alicePublicKeyBytes.ToArray()).Replace("-", "");
            Console.WriteLine(alicePublicKeyString);

            // Виводимо приватний ключ Alice у форматі PKCS#8
            Console.WriteLine("\nAlice's Private Key:");
            var alicePrivateKeyBytes = rsaAlice.ExportPkcs8PrivateKey();

            var alicePrivateKeyString = BitConverter.ToString(alicePrivateKeyBytes.ToArray()).Replace("-", "");
            //MessageBox.Show(alicePrivateKeyString);

            // Виводимо публічний ключ Bob у форматі SubjectPublicKeyInfo
            Console.WriteLine("\nBob's Public Key:");
            var bobPublicKeyBytes = rsaBob.ExportSubjectPublicKeyInfo();
            var bobPublicKeyString = BitConverter.ToString(bobPublicKeyBytes.ToArray()).Replace("-", "");
            Console.WriteLine(bobPublicKeyString);

            // Виводимо приватний ключ Bob у форматі PKCS#8
            Console.WriteLine("\nBob's Private Key:");
            var bobPrivateKeyBytes = rsaBob.ExportPkcs8PrivateKey();
            var bobPrivateKeyString = BitConverter.ToString(bobPrivateKeyBytes.ToArray()).Replace("-", "");
            Console.WriteLine(bobPrivateKeyString);

        }


     
    }  
}
