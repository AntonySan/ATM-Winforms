using ATM_APP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_Winforms
{
    public class User
    {
        public int ID { get; set; }
        public string CardNumber { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string ExpirationDate { get; set; }
        public string PaymentSystem { get; set; }
        public string Balance { get; set; }
        public string Address { get; set; }
        public string IssueDate { get; set; }
        public string CVV_CVC { get; set; }
        public string CardStatus { get; set; }
        public string SpendingLimit { get; set; }
        public string IssuingBank { get; set; }
        public string CardType { get; set; }

        // Конструктор класу
        public User(int id, string cardnumber, string name, string password, string expirationdate, string paymentsystem, string balance, string address, string issuedate, string cvv_cvc, string cardstatus, string spendinglimit, string issuingbank, string cardtype)
        {
            ID = id;
            CardNumber = cardnumber;
            FullName = name;
            Password = password;
            ExpirationDate = expirationdate;
            PaymentSystem = paymentsystem;
            Balance = balance;
            Address = address;
            IssueDate = issuedate;
            CVV_CVC = cvv_cvc;
            CardStatus = cardstatus;
            SpendingLimit = spendinglimit;
            IssuingBank = issuingbank;
            CardType = cardtype;
        }
    }

    public static class GlobalData
    {
        public static List<User> Users { get; } = new List<User>();
        // Метод для очищення списку користувачів
        public static void ClearUsers()
        {
            Users.Clear();
        }
    }

}
