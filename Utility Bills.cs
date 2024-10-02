using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data.SqlClient;
using ATM_CryptoGuardian;
using System.Security.Cryptography;

namespace ATM_APP
{
    // *****************************************************************
    //                          Клас Utility_Bills
    // Описує рахунок за комунальні послуги
    // *****************************************************************
    public class Utility_Bills
    {
        // Властивості класу Utility_Bills
        public int Id { get; set; }
        public string User_Name { get; set; }
        public string Company_Name { get; set; }
        public int Amount { get; set; }
        public string Address { get; set; }
        public string Tarif { get; set; }
        public string Used { get; set; }
        public string Paid { get; set; }

        // Конструктор класу Utility_Bills
        public Utility_Bills(int id, string user_name, string company_name, int amount, string address, string tarif, string used, string paid)
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

    // *****************************************************************
    //                    Клас GlobalUtility_Bills
    // Управляє глобальним списком рахунків за комунальні послуги
    // *****************************************************************
    public class GlobalUtility_Bills
    {
        // Глобальний список рахунків за комунальні послуги
        public static List<Utility_Bills> utility_Bills { get; } = new List<Utility_Bills>();

        // Метод для очищення списку рахунків
        public static void ClearUtility_Bills()
        {
            utility_Bills.Clear();
        }

        // Метод для отримання всіх рахунків користувачів з бази даних
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
                    while (reader.Read())
                    {
                        // Отримання даних з результатів запиту
                        int id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : (int)reader["Id"];
                        string userFullName = reader.IsDBNull(reader.GetOrdinal("Cardholder_Name")) ? string.Empty : (string)reader["Cardholder_Name"];
                        string address = reader.IsDBNull(reader.GetOrdinal("Address")) ? string.Empty : (string)reader["Address"];
                        string companyName = reader.IsDBNull(reader.GetOrdinal("Compani_name")) ? string.Empty : (string)reader["Compani_name"];
                        int amountToPay = reader.IsDBNull(reader.GetOrdinal("Amount")) ? 0 : (int)reader["Amount"];
                        string tariff = reader.IsDBNull(reader.GetOrdinal("tariff")) ? string.Empty : (string)reader["tariff"];
                        string used = reader.IsDBNull(reader.GetOrdinal("used")) ? string.Empty : (string)reader["used"];
                        string isPaid = reader.IsDBNull(reader.GetOrdinal("paid")) ? string.Empty : (string)reader["paid"];

                        // Створення об'єкту з отриманих даних і додавання його до списку
                        Utility_Bills userData = new Utility_Bills(id, userFullName, companyName, amountToPay, address, tariff, used, isPaid);
                        GlobalUtility_Bills.utility_Bills.Add(userData);
                    }
                }
            }
        }





    }
}
