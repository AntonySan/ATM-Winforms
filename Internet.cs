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
    public class Internet
    {
        public int Id { get; set; }
        public string Account_Number { get; set; }
        public string Address { get; set; }
        public int Paid { get; set; }
        public int Transfer_Amount { get; set; }
        public string Tariff_Plan { get; set; }
        public string Payment_Date { get; set; }
        public string Service_Status { get; set; }
        public string Data_Usage { get; set; }
        public string User_Name { get; set; }

        public Internet(int id, string account_Number, string address, int paid, int transfer_Amount, string tariff_Plan, string payment_Date, string service_Status, string data_Usage, string user_Name)
        {
            Id = id;
            Account_Number = account_Number;
            Address = address;
            Paid = paid;
            Transfer_Amount = transfer_Amount;
            Tariff_Plan = tariff_Plan;
            Payment_Date = payment_Date;
            Service_Status = service_Status;
            Data_Usage = data_Usage;
            User_Name = user_Name;
        }
    }

    public static class GlobalInternetData
    {
        public static List<Internet> internet { get; } = new List<Internet>();

        // Метод для очищення списку користувачів
        public static void ClearInternet()
        {
            internet.Clear();
        }

        public static void GetAllInternetData()
        {
            GlobalInternetData.ClearInternet(); // Очищаємо список, щоб заповнити його новими даними

            using (SqlConnection connection = new SqlConnection(Resource_Paths.DB_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Internet";
                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = (int)reader["Id"];
                        string accNumber = (string)reader["Account_number"];
                        string address = (string)reader["Address"];
                        int paid = (int)reader["Paid"];
                        int transferAmount = (int)reader["Transfer_Amount"];
                        string tariffPlan = (string)reader["Tariff_Plan"];
                        string paymentDate = (string)reader["Payment_Date"];
                        string serviceStatus = (string)reader["Service_Status"];
                        string dataUsage = (string)reader["Data_Usage"];
                        string userName = (string)reader["User_Name"];

                        Internet internetData = new Internet(id, accNumber, address, paid, transferAmount, tariffPlan, paymentDate, serviceStatus, dataUsage, userName);
                        GlobalInternetData.internet.Add(internetData);
                    }
                }
            }
        }


    }
}
