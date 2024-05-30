using ATM_APP;
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

                SqlDataReader reader = cmd.ExecuteReader();

                // Завантаження приватного ключа для дешифрування
                RSAParameters privateKey = RSAKeyManager.LoadPrivateKey();

                // Читання результатів запиту
                while (reader.Read())
                {
                    int id = (int)reader["Id"];

                    // Дешифрування полів
                    string retrievedCardNumber = DecryptField(reader["Card_number"], privateKey);
                    string name = DecryptField(reader["Full_name"], privateKey);
                    string retrievedPassword = DecryptField(reader["PIN"], privateKey);
                    string expirationDate = DecryptField(reader["Expiration_date"], privateKey);
                    string paymentSystem = DecryptField(reader["Payment_system"], privateKey);
                    string balance = DecryptField(reader["balance"], privateKey);
                    string address = DecryptField(reader["address"], privateKey);
                    string issueDate = DecryptField(reader["Issue_Date"], privateKey);
                    string cvv_cvc = DecryptField(reader["CVV/CVC "], privateKey);
                    string cardStatus = DecryptField(reader["Card_Status"], privateKey);
                    string spendingLimit = DecryptField(reader["Spending_Limit"], privateKey);
                    string issuingBank = DecryptField(reader["Issuing_bank"], privateKey);
                    string cardType = DecryptField(reader["Card_Type"], privateKey);

                    // Створюємо об'єкт користувача та додаємо його до списку
                    User user = new User(id, retrievedCardNumber, name, retrievedPassword, expirationDate, paymentSystem, balance, address, issueDate, cvv_cvc, cardStatus, spendingLimit, issuingBank, cardType);
                    GlobalData.Users.Add(user);
                }

                reader.Close();
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
