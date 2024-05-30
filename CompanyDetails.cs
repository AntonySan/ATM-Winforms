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
    public class CompanyDetails
    {
        public int Id { get; set; }
        public string Company_Name { get; set; }
        public string IBAN { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string TIN { get; set; }
        public string EDRPOU { get; set; }
        public int AccountBalance { get; set; }
       
        public CompanyDetails(int id, string company_name, string iban, string country,
            string address, string contactpersom, string phone, string tin,
            string edrpou, int accountbalance)
        {
            Id = id;
            Company_Name = company_name;
            IBAN = iban;
            Country = country;
            Address = address;
            ContactPerson = contactpersom;
            Phone = phone;
            TIN = tin;
            EDRPOU = edrpou;
            AccountBalance = accountbalance;
            


        }

        public static class GlobalCompanyDetails
        {
            public static List<CompanyDetails> companies { get; } = new List<CompanyDetails>();

            // Метод для очищення списку реєстрації карток
            public static void ClearCompanyDetails()
            {
                companies.Clear();
            }
            public static void GetAllCompanyDetails()
            {
                using (SqlConnection conn = new SqlConnection(Resource_Paths.DB_connectionString))
                {
                    conn.Open();

                    // Завантаження приватного ключа для дешифрування
                    RSAParameters privateKey = RSAKeyManager.LoadPrivateKey();

                    string query = "SELECT * FROM CompanyDetails";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = (int)reader["Id"];
                            string companyName = DecryptField(reader["CompanyName"], privateKey);
                            string IBAN = DecryptField(reader["IBAN"], privateKey);
                            string country = DecryptField(reader["Country"], privateKey);
                            string address = DecryptField(reader["Address"], privateKey);
                            string contactPerson = DecryptField(reader["ContactPerson"], privateKey);
                            string phone = DecryptField(reader["Phone"], privateKey);
                            string tin = DecryptField(reader["TIN"], privateKey);
                            string edrpou = DecryptField(reader["EDRPOU"], privateKey);
                            int accountBalance = Convert.ToInt32(DecryptField(reader["AccountBalance"], privateKey));

                            // Створення нового об'єкта CompanyDetails з використанням конструктора з параметрами
                            CompanyDetails company = new CompanyDetails(id, companyName, IBAN, country, address, contactPerson, phone, tin, edrpou, accountBalance);
                            GlobalCompanyDetails.companies.Add(company);
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
}
