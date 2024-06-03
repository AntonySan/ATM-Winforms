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

                    string query = "SELECT * FROM CompanyDetails";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = (int)reader["Id"];
                            string companyName = (string)reader["CompanyName"];
                            string IBAN = (string)reader["IBAN"];
                            string country = (string)reader["Country"];
                            string address = (string)reader["Address"];
                            string contactPerson = (string)reader["ContactPerson"];
                            string phone = (string)reader["Phone"];
                            string tin = (string)reader["TIN"];
                            string edrpou = (string)reader["EDRPOU"];
                            int accountBalance = (int)reader["AccountBalance"];

                            CompanyDetails company = new CompanyDetails(id, companyName, IBAN, country, address, contactPerson, phone, tin, edrpou, accountBalance);
                            GlobalCompanyDetails.companies.Add(company);
                        }
                    }
                }
            }


        }
    }
}
