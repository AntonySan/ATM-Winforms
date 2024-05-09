using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}