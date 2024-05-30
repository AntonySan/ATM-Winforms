using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ATM_CryptoGuardian;
using System.Security.Cryptography;

namespace ATM_Winforms
{
    public class CharityFond
    {
        public int Id { get; set; }
        public string FondName { get; set; }
        public string RegistrationNumber { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string BankAccount { get; set; }
        public decimal AccountBalance { get; set; }

        public CharityFond(int id, string fondName, string registrationNumber, string country,
                           string address, string contactPerson, string phone, string email,
                           string bankAccount, decimal accountBalance)
        {
            Id = id;
            FondName = fondName;
            RegistrationNumber = registrationNumber;
            Country = country;
            Address = address;
            ContactPerson = contactPerson;
            Phone = phone;
            Email = email;
            BankAccount = bankAccount;
            AccountBalance = accountBalance;
        }
    }

    public static class GlobalCharityFond
    {
        public static List<CharityFond> CharityFonds { get; } = new List<CharityFond>();

        // Метод для очищення списку фондів
        public static void ClearCharityFonds()
        {
            CharityFonds.Clear();
        }

        public static void GetAllCharityFond()
        {
            using (SqlConnection conn = new SqlConnection(Resource_Paths.DB_connectionString))
            {
                conn.Open();

                // Завантаження приватного ключа для дешифрування
                RSAParameters privateKey = RSAKeyManager.LoadPrivateKey();

                string query = "SELECT * FROM CharityFond";
                SqlCommand cmd = new SqlCommand(query, conn);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = (int)reader["Id"];
                        string fondName = DecryptField(reader["FondName"], privateKey);
                        string registrationNumber = DecryptField(reader["RegistrationNumber"], privateKey);
                        string country = DecryptField(reader["Country"], privateKey);
                        string address = DecryptField(reader["Address"], privateKey);
                        string contactPerson = DecryptField(reader["ContactPerson"], privateKey);
                        string phone = DecryptField(reader["Phone"], privateKey);
                        string email = DecryptField(reader["Email"], privateKey);
                        string bankAccount = DecryptField(reader["BankAccount"], privateKey);
                        decimal accountBalance = Convert.ToDecimal(DecryptField(reader["AccountBalance"], privateKey));

                        CharityFond fond = new CharityFond(id, fondName, registrationNumber, country, address, contactPerson, phone, email, bankAccount, accountBalance);
                        GlobalCharityFond.CharityFonds.Add(fond);
                    }
                }
            }
        }
        private static string DecryptField(object encryptedField, RSAParameters privateKey)
        {
            if (encryptedField == null || encryptedField == DBNull.Value)
            {
                return null;
            }

            byte[] encryptedData = Convert.FromBase64String(encryptedField.ToString());
            byte[] decryptedData = Encryption_Manager.DecryptData(encryptedData, privateKey);
            return Encoding.UTF8.GetString(decryptedData);
        }

    }
}