 using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data.OleDb;
using ATM_Winforms;
using Microsoft.VisualBasic;
using static ATM_Winforms.CompanyDetails;
using ATM_CryptoGuardian;
using System.Security.Cryptography;
using System.Text;

namespace ATM_APP
{
   public  class DatabaseManager
    {
      
        public static void ConnectToDatabase(Form form)
        {
            string connString = "Data Source=.\\sqlexpress;Initial Catalog=ATM;Integrated Security=True";

            // Creating an OdbcConnection object to connect to the Access database
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to connect to the database: " + ex.Message, "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public static List<Dictionary<string, object>> ReadData(SqlConnection sqlConnection, string tableName, List<string> fields)
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

            string sqlQuery = "SELECT ";

            // Додаємо поля до SQL-запиту
            foreach (string field in fields)
            {
                sqlQuery += field + ", ";
            }
            sqlQuery = sqlQuery.Remove(sqlQuery.Length - 2); // Видаляємо останню кому

            sqlQuery += " FROM " + tableName;

            SqlCommand command = new SqlCommand(sqlQuery, sqlConnection);

            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();
                    foreach (string field in fields)
                    {
                        row[field] = reader[field];
                    }
                    results.Add(row);
                }
            }

            return results;
        }



        public static void WriteData(SqlConnection sqlConnection, string tableName, Dictionary<string, ExchangeRateData> parameters)
        {
            sqlConnection.Open();

            foreach (var parameter in parameters)
            {
                string mergeQuery = $"MERGE INTO {tableName} AS target " +
                     $"USING (VALUES " +
                     $"(@Date, @Bank, @BaseCurrency, @BaseCurrencyLit, @ExchangeRate, @Currency, @SaleRateNB, @PurchaseRateNB, @SaleRate, @PurchaseRate)) " +
                     $"AS source (Date, Bank, baseCurrency, baseCurrencyLit, exchangeRate, currency, SaleRateNB, PurchaseRateNB, SaleRate, PurchaseRate) " +
                     $"ON (target.Bank = source.Bank AND target.baseCurrency = source.baseCurrency AND target.currency = source.currency) " +
                     $"WHEN MATCHED THEN " +
                     $"UPDATE SET " +
                     $"target.Date = source.Date, " + // Оновлення дати
                     $"target.baseCurrencyLit = source.baseCurrencyLit, " +
                     $"target.exchangeRate = source.exchangeRate, " +
                     $"target.SaleRateNB = source.SaleRateNB, " +
                     $"target.PurchaseRateNB = source.PurchaseRateNB, " +
                     $"target.SaleRate = source.SaleRate, " +
                     $"target.PurchaseRate = source.PurchaseRate " +
                     $"WHEN NOT MATCHED THEN " +
                     $"INSERT (Date, Bank, baseCurrency, baseCurrencyLit, exchangeRate, currency, SaleRateNB, PurchaseRateNB, SaleRate, PurchaseRate) " +
                     $"VALUES (source.Date, source.Bank, source.baseCurrency, source.baseCurrencyLit, source.exchangeRate, source.currency, source.SaleRateNB, source.PurchaseRateNB, source.SaleRate, source.PurchaseRate);";

                using (SqlCommand command = new SqlCommand(mergeQuery, sqlConnection))
                {
                    command.Parameters.AddWithValue("@Date", parameter.Value.Date);
                    command.Parameters.AddWithValue("@Bank", parameter.Value.Bank);
                    command.Parameters.AddWithValue("@BaseCurrency", parameter.Value.BaseCurrency);
                    command.Parameters.AddWithValue("@BaseCurrencyLit", parameter.Value.BaseCurrencyLit);
                    command.Parameters.AddWithValue("@ExchangeRate", parameter.Value.ExchangeRate);
                    command.Parameters.AddWithValue("@Currency", parameter.Value.Currency);
                    command.Parameters.AddWithValue("@SaleRateNB", parameter.Value.SaleRateNB);
                    command.Parameters.AddWithValue("@PurchaseRateNB", parameter.Value.PurchaseRateNB);
                    command.Parameters.AddWithValue("@SaleRate", parameter.Value.SaleRate);
                    command.Parameters.AddWithValue("@PurchaseRate", parameter.Value.PurchaseRate);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                       
                    }
                    else
                    {
                        MessageBox.Show("Error. Data has not been successfully updated");
                    }
                }
            }
        }

        public  void UpdateCardData(string cardNumber, string cardType, string issueDate, string expirationDate, string cardholderName, string cardholderAddress, string cvvCvc, string pin, int accountBalance, string cardStatus, string paymentSystem, string spendingLimit, string issuingBank)
        {
            using (SqlConnection conn = new SqlConnection(Resource_Paths.DB_connectionString))
            {
                conn.Open();

                string query = @"UPDATE CardRegistry SET 
                        Card_Type = @CardType, 
                        Issue_Date = @IssueDate, 
                        Expiration_Date = @ExpirationDate, 
                        Cardholder_Name = @CardholderName, 
                        [Cardholder's_Address:] = @CardholderAddress, 
                        [CVV/CVC] = @CVVCVC, 
                        PIN = @PIN, 
                        Account_Balance = @AccountBalance, 
                        Card_Status = @CardStatus, 
                        Payment_System = @PaymentSystem, 
                        Spending_Limit = @SpendingLimit, 
                        Issuing_bank = @IssuingBank 
                        WHERE Card_Number = @CardNumber";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CardType", cardType);
                cmd.Parameters.AddWithValue("@IssueDate", issueDate);
                cmd.Parameters.AddWithValue("@ExpirationDate", expirationDate);
                cmd.Parameters.AddWithValue("@CardholderName", cardholderName);
                cmd.Parameters.AddWithValue("@CardholderAddress", cardholderAddress);
                cmd.Parameters.AddWithValue("@CVVCVC", cvvCvc);
                cmd.Parameters.AddWithValue("@PIN", pin);
                cmd.Parameters.AddWithValue("@AccountBalance", accountBalance);
                cmd.Parameters.AddWithValue("@CardStatus", cardStatus);
                cmd.Parameters.AddWithValue("@PaymentSystem", paymentSystem);
                cmd.Parameters.AddWithValue("@SpendingLimit", spendingLimit);
                cmd.Parameters.AddWithValue("@IssuingBank", issuingBank);
                cmd.Parameters.AddWithValue("@CardNumber", cardNumber);

                cmd.ExecuteNonQuery();
            }
        }

        // Метод для завантаження даних з бази даних за номером картки
        public static void LoadCardRegistriesFromDatabase(string cardNumber)
        {
            using (SqlConnection conn = new SqlConnection(Resource_Paths.DB_connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM CardRegistry WHERE Card_Number = @CardNumber";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CardNumber", cardNumber);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int id = (int)reader["Id"];
                        string cardType = reader["Card_Type"].ToString();
                        string issueDate = reader["Issue_Date"].ToString();
                        string expirationDate = reader["Expiration_Date"].ToString();
                        string cardholderName = reader["Cardholder_Name"].ToString();
                        string cardholderAddress = reader["Cardholder's_Address:"].ToString();
                        string cvvCvc = reader["CVV/CVC "].ToString();
                        string pin = reader["PIN"].ToString();
                        int accountBalance = (int)reader["Account_Balance"];
                        string cardStatus = reader["Card_Status"].ToString();
                        string paymentSystem = reader["Payment_System"].ToString();
                        string spendingLimit = reader["Spending_Limit"].ToString();
                        string issuingBank = reader["Issuing_bank"].ToString();

                        // Створення нового об'єкта Card_Registry з використанням конструктора з параметрами
                        Card_Registry card = new Card_Registry(id, cardNumber, cardType, issueDate, expirationDate, cardholderName, cardholderAddress, cvvCvc, pin, accountBalance, cardStatus, paymentSystem, spendingLimit, issuingBank);
                        GlobalCardlData.CardRegistries.Add(card);
                    }
                    else
                    {
                        MessageBox.Show("Картку з номером {0} не знайдено в базі даних.", cardNumber);
                        // Або виконати іншу логіку за вашим бажанням
                    }
                }
            }
        }


        public void UpdateCompanyData(string iban, string companyName, string country, string address, string contactPerson, string phone, string tin, string edrpou, int accountBalance)
        {
            using (SqlConnection conn = new SqlConnection(Resource_Paths.DB_connectionString))
            {
                conn.Open();

                string query = @"UPDATE CompanyDetails SET 
                            CompanyName = @CompanyName, 
                            Country = @Country, 
                            Address = @Address, 
                            ContactPerson = @ContactPerson, 
                            Phone = @Phone, 
                            TIN = @TIN, 
                            EDRPOU = @EDRPOU, 
                            AccountBalance = @AccountBalance 
                        WHERE IBAN = @IBAN";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CompanyName", companyName);
                cmd.Parameters.AddWithValue("@Country", country);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@ContactPerson", contactPerson);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@TIN", tin);
                cmd.Parameters.AddWithValue("@EDRPOU", edrpou);
                cmd.Parameters.AddWithValue("@AccountBalance", accountBalance);
                cmd.Parameters.AddWithValue("@IBAN", iban);

                cmd.ExecuteNonQuery();
            }
        }

        public static void LoadCompanyDetailsFromDatabase(string iban)
        {
            using (SqlConnection conn = new SqlConnection(Resource_Paths.DB_connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM CompanyDetails WHERE IBAN = @IBAN";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IBAN", iban);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int id = (int)reader["Id"];
                        string companyName = reader["CompanyName"].ToString();
                        string IBAN = reader["IBAN"].ToString();
                        string country = reader["Country"].ToString();
                        string address = reader["Address"].ToString();
                        string contactPerson = reader["ContactPerson"].ToString();
                        string phone = reader["Phone"].ToString();
                        string tin = reader["TIN"].ToString();
                        string edrpou = reader["EDRPOU"].ToString();
                        int accountBalance = Convert.ToInt32(reader["AccountBalance"]);

                        // Створення нового об'єкта Card_Registry з використанням конструктора з параметрами
                        CompanyDetails company = new CompanyDetails(id, companyName, IBAN, country, address, contactPerson, phone, tin, edrpou, accountBalance);
                        GlobalCompanyDetails.companies.Add(company);

                    }
                    else
                    {
                        MessageBox.Show(string.Format("Компанію з IBAN {0} не знайдено в базі даних.", iban));
                        // Або виконати іншу логіку за вашим бажанням
                    }
                }
            }
        }
       


        public void UpdateCharityFondData(string fondName, string registrationNumber, string country, string address, string contactPerson, string phone, string email, string bankAccount, decimal accountBalance)
        {
            using (SqlConnection conn = new SqlConnection(Resource_Paths.DB_connectionString))
            {
                conn.Open();

                string query = @"UPDATE CharityFond SET 
                         RegistrationNumber = @RegistrationNumber, 
                         Country = @Country, 
                         Address = @Address, 
                         ContactPerson = @ContactPerson, 
                         Phone = @Phone, 
                         Email = @Email, 
                         BankAccount = @BankAccount, 
                         AccountBalance = @AccountBalance 
                     WHERE FondName = @FondName";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FondName", fondName);
                cmd.Parameters.AddWithValue("@RegistrationNumber", registrationNumber);
                cmd.Parameters.AddWithValue("@Country", country);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@ContactPerson", contactPerson);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@BankAccount", bankAccount);
                cmd.Parameters.AddWithValue("@AccountBalance", accountBalance);

                cmd.ExecuteNonQuery();
            }
        }

        public static void LoadCharityFondFromDatabase(string fondName)
        {
            using (SqlConnection conn = new SqlConnection(Resource_Paths.DB_connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM CharityFond WHERE FondName = @FondName";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FondName", fondName);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int id = (int)reader["Id"];
                        string registrationNumber = reader["RegistrationNumber"].ToString();
                        string country = reader["Country"].ToString();
                        string address = reader["Address"].ToString();
                        string contactPerson = reader["ContactPerson"].ToString();
                        string phone = reader["Phone"].ToString();
                        string email = reader["Email"].ToString();
                        string bankAccount = reader["BankAccount"].ToString();
                        decimal accountBalance = Convert.ToDecimal(reader["AccountBalance"]);

                        CharityFond fond = new CharityFond(id, fondName, registrationNumber, country, address, contactPerson, phone, email, bankAccount, accountBalance);
                        GlobalCharityFond.CharityFonds.Add(fond);
                    }
                    else
                    {
                        MessageBox.Show($"Фонд з назвою '{fondName}' не знайдено в базі даних.");
                        // Або виконати іншу логіку за вашим бажанням
                    }
                }
            }
        }


       
        public static async Task InsertTransaction(string connectionString, int userId, string transactionType, decimal amount, string currency, DateTime timestamp, string status, string? sourceAccountId, string? destinationAccountId, string description)
        {
         
            string query = @"INSERT INTO Transactions 
                     ([user_id], [transaction_type], [amount], [currency], [timestamp], [status], [source_account], [destination_account], [description])
                     VALUES
                     (@userId, @transactionType, @amount, @currency, @timestamp, @status, @sourceAccount, @destinationAccount, @description)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
            
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@transactionType", transactionType);
                    command.Parameters.AddWithValue("@amount", amount);
                    command.Parameters.AddWithValue("@currency", currency);
                    command.Parameters.AddWithValue("@timestamp", timestamp);
                    command.Parameters.AddWithValue("@status", status);
                    command.Parameters.AddWithValue("@sourceAccount", (object)sourceAccountId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@destinationAccount", (object)destinationAccountId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@description", (object)description ?? DBNull.Value);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                     
                    }
                    catch (SqlException ex)
                    {
                       MessageBox.Show("SQL помилка: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка: " + ex.Message);
                    }
                    finally
                    {
                        connection.Close(); // Закриття підключення навіть у випадку виникнення виключення
                    }
                }
            }
        }


        

       


    }
}


