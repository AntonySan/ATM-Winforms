using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ATM_CryptoGuardian;
using System.Security.Cryptography;

namespace ATM_APP
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

                string query = "SELECT * FROM CharityFond";
                SqlCommand cmd = new SqlCommand(query, conn);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = (int)reader["Id"];
                        string fondName = (string)reader["FondName"];
                        string registrationNumber = (string)reader["RegistrationNumber"];
                        string country = (string)reader["Country"];
                        string address = (string)reader["Address"];
                        string contactPerson = (string)reader["ContactPerson"];
                        string phone = (string)reader["Phone"];
                        string email = (string)reader["Email"];
                        string bankAccount = (string)reader["BankAccount"];
                        decimal accountBalance = (decimal)reader["AccountBalance"];

                        CharityFond fond = new CharityFond(id, fondName, registrationNumber, country, address, contactPerson, phone, email, bankAccount, accountBalance);
                        GlobalCharityFond.CharityFonds.Add(fond);
                    }
                }
            }
        }

       

    }
}