using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data.SqlClient;
using ATM_CryptoGuardian;
using System.Security.Cryptography;

namespace ATM_Winforms
{
    public class Utility_Bills
    {
        public int Id {  get; set; }
        public string User_Name { get; set; }
        public string Company_Name { get; set; }
        public int Amount { get; set; }
        public string Address {  get; set; }

        public string Tarif { get; set; }
        public string Used {  get; set; }
        public string Paid {  get; set; }

        public Utility_Bills(int id,string user_name,string company_name, int amount,string address,string tarif, string used,string paid)
        {
            Id = id;
            User_Name = user_name;
            Company_Name = company_name;
            Amount = amount;
            Address = address;
            Tarif = tarif;
            Used = used;
            Paid = paid;

        }
    }

    public class GlobalUtility_Bills
    {
        public static List<Utility_Bills> utility_Bills { get; } = new List<Utility_Bills>();

        public static void ClearUtility_Bills()
        {
            utility_Bills.Clear();
        }


        public static void GetAllUserBills()
        {
            string connectionString = Resource_Paths.DB_connectionString; // Замініть на ваш рядок підключення

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
        SELECT uc.Id, cr.Cardholder_Name, uc.Compani_name, uc.Amount, uc.Address, uc.tariff, uc.used, uc.paid
        FROM Utility_Сompanies uc
        LEFT JOIN CardRegistry cr ON uc.Address = cr.[Cardholder's_Address:]";

                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Завантаження приватного ключа для дешифрування
                    RSAParameters privateKey = RSAKeyManager.LoadPrivateKey();

                    while (reader.Read())
                    {
                        // Отримання даних з результатів запиту
                        int id = (int)reader["Id"];

                        // Дешифрування полів
                        string userFullName = DecryptField(reader["Cardholder_Name"], privateKey);
                        string address = DecryptField(reader["Address"], privateKey);
                        string companyName = DecryptField(reader["Compani_name"], privateKey);
                        int amountToPay = Convert.ToInt32(DecryptField(reader["Amount"], privateKey));
                        string tariff = DecryptField(reader["tariff"], privateKey);
                        string used = DecryptField(reader["used"], privateKey);
                        string isPaid = DecryptField(reader["paid"], privateKey);

                        // Створення об'єкту з отриманих даних і додавання його до списку
                        Utility_Bills userData = new Utility_Bills(id, userFullName, companyName, amountToPay, address, tariff, used, isPaid);
                        GlobalUtility_Bills.utility_Bills.Add(userData);
                    }
                }
            }
        }

        // Метод для дешифрування окремих полів
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
