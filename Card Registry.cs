using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_Winforms
{
    public class Card_Registry
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string CardType { get; set; }
        public string IssueDate { get; set; }
        public string ExpirationDate { get; set; }
        public string CardholderName { get; set; }
        public string CardholderAddress { get; set; }
        public string CVV_CVC { get; set; }
        public string PIN { get; set; }
        public int AccountBalance { get; set; }
        public string CardStatus { get; set; }
        public string PaymentSystem { get; set; }
        public string SpendingLimit { get; set; }
        public string IssuingBank { get; set; }


        public Card_Registry(int id, string cardnumber, string cardtype, string issuedate, 
            string expirationdate, string cardholdername, string cardholderaddress, string cvv_cvc, 
            string pin, int accountbalance,string cardstatus, string paymentsystem, string spendinglimit, string issuingbank)
        { 
             Id = id; 
            CardNumber = cardnumber;
            CardType = cardtype;
            IssueDate = issuedate;
            ExpirationDate = expirationdate;
            CardholderName = cardholdername;
            CardholderAddress = cardholderaddress;
            CVV_CVC = cvv_cvc;
            PIN = pin;
            AccountBalance = accountbalance;
            CardStatus = cardstatus;
            PaymentSystem = paymentsystem;
            SpendingLimit = spendinglimit;
            IssuingBank = issuingbank;


        }


    }

    public static class GlobalCardlData
    {
        public static List<Card_Registry> CardRegistries { get; } = new List<Card_Registry>();

        // Метод для очищення списку реєстрації карток
        public static void ClearCardRegistries()
        {
            CardRegistries.Clear();
        }
    }


}
