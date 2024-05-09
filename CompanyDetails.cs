using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}
