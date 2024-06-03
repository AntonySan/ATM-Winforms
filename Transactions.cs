using ATM_CryptoGuardian;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace ATM_Winforms
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        public string TransactionType { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
        public string SourceAccount { get; set; }
        public string DestinationAccount { get; set; }
        public string Description { get; set; }

        // Конструктор класу
        public Transaction(int transactionId, int userId, string transactionType, int amount, string currency, DateTime timestamp, string status, string sourceAccount, string destinationAccount, string description)
        {
            TransactionId = transactionId;
            UserId = userId;
            TransactionType = transactionType;
            Amount = amount;
            Currency = currency;
            Timestamp = timestamp;
            Status = status;
            SourceAccount = sourceAccount;
            DestinationAccount = destinationAccount;
            Description = description;
        }
    }

    public static class GlobalTransactionData
    {
        public static List<Transaction> Transactions { get; } = new List<Transaction>();

        // Метод для очищення списку транзакцій
        public static void ClearTransactions()
        {
            Transactions.Clear();
        }

        // Метод для отримання даних з таблиці Transactions на основі user_id
        public static void LoadTransactions(int userId)
        {
            ClearTransactions();
            using (SqlConnection conn = new SqlConnection(Resource_Paths.DB_connectionString))
            {
                conn.Open();

                // Запит з умовою вибору транзакцій за останній тиждень
                string query = @"
            SELECT * 
            FROM Transactions 
            WHERE user_id = @UserId 
              AND timestamp >= DATEADD(day, -7, GETDATE())";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int transactionId = (int)reader["transaction_id"];
                    int uid = (int)reader["user_id"];
                    string transactionType = reader["transaction_type"].ToString();
                    int amount = (int)reader["amount"];
                    string currency = reader["currency"].ToString();
                    DateTime timestamp = (DateTime)reader["timestamp"];
                    string status = reader["status"].ToString();
                    string sourceAccount = reader["source_account"]?.ToString();
                    string destinationAccount = reader["destination_account"]?.ToString();
                    string description = reader["description"]?.ToString();

                    Transaction transaction = new Transaction(transactionId, uid, transactionType, amount, currency, timestamp, status, sourceAccount, destinationAccount, description);
                    Transactions.Add(transaction);
                }

                reader.Close();
            }
        }

        public static void GetAllTransactions()
        {
            ClearTransactions();
            using (SqlConnection conn = new SqlConnection(Resource_Paths.DB_connectionString))
            {
                conn.Open();

                // Запит для вибору всіх транзакцій користувача
                string query = "SELECT * FROM Transactions";
                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int transactionId = reader["transaction_id"] != DBNull.Value ? (int)reader["transaction_id"] : 0;
                    int uid = reader["user_id"] != DBNull.Value ? (int)reader["user_id"] : 0;
                    string transactionType = reader["transaction_type"] != DBNull.Value ? (string)reader["transaction_type"] : string.Empty;
                    int amount = reader["amount"] != DBNull.Value ? (int)reader["amount"] : 0;
                    string currency = reader["currency"] != DBNull.Value ? (string)reader["currency"] : string.Empty;
                    DateTime timestamp = reader["timestamp"] != DBNull.Value ? (DateTime)reader["timestamp"] : DateTime.MinValue;
                    string status = reader["status"] != DBNull.Value ? (string)reader["status"] : string.Empty;
                    string sourceAccount = reader["source_account"] != DBNull.Value ? (string)reader["source_account"] : string.Empty;
                    string destinationAccount = reader["destination_account"] != DBNull.Value ? (string)reader["destination_account"] : string.Empty;
                    string description = reader["description"] != DBNull.Value ? (string)reader["description"] : string.Empty;

                    Transaction transaction = new Transaction(transactionId, uid, transactionType, amount, currency, timestamp, status, sourceAccount, destinationAccount, description);
                    Transactions.Add(transaction);
                }

                reader.Close();
            }
        }





    }
}
