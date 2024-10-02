using ATM_APP;
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
        public static void GetAllUser()
        {
            using (SqlConnection conn = new SqlConnection(Resource_Paths.DB_connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM ATM_info";
                SqlCommand cmd = new SqlCommand(query, conn);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = (int)reader["Id"];
                        string retrievedCardNumber = (string)reader["Card_number"];
                        string name = (string)reader["Full_name"];
                        string retrievedPassword = (string)reader["PIN"];
                        string expirationDate = (string)reader["Expiration_date"];
                        string paymentSystem = (string)reader["Payment_system"];
                        int balance = Convert.ToInt32(reader["balance"]);
                        string address = (string)reader["address"];
                        string issueDate = (string)reader["Issue_Date"];
                        string cvv_cvc = (string)reader["CVV/CVC "];
                        string cardStatus = (string)reader["Card_Status"];
                        string spendingLimit = (string)reader["Spending_Limit"];
                        string issuingBank = (string)reader["Issuing_bank"];
                        string cardType = (string)reader["Card_Type"];

                        User user = new User(id, retrievedCardNumber, name, retrievedPassword, expirationDate, paymentSystem, balance.ToString(), address, issueDate, cvv_cvc, cardStatus, spendingLimit, issuingBank, cardType);
                        GlobalData.Users.Add(user);
                    }
                }
            }
        }

    }

}
