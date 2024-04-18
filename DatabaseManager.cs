 using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace ATM_APP
{
    internal class DatabaseManager
    {
         void ConnectToDatabase(Form form)
        {
            const string connString = "Data Source=.\\sqlexpress;Initial Catalog=ATM;Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connString); ;
            try
            {
                 sqlConnection.Open();

            }
            catch(Exception ex) 
            {
                MessageBox.Show("Failed to connect to database","Database Connection Error", MessageBoxButtons.OK);
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        void ReadData(SqlConnection sqlConnection, string tableName, Dictionary<string, object> parameters)
        {
            // Створюємо SQL-запит з використанням переданої назви таблиці та параметрів
            string sqlQuery = "SELECT * FROM " + tableName + " WHERE ";
            foreach (var parameter in parameters)
            {
                sqlQuery += parameter.Key + " = @" + parameter.Key + " AND ";
            }
            // Видаляємо "AND " в кінці запиту
            sqlQuery = sqlQuery.Remove(sqlQuery.Length - 5);

            SqlCommand command = new SqlCommand(sqlQuery, sqlConnection);

            // Додаємо параметри до команди SQL
            foreach (var parameter in parameters)
            {
                command.Parameters.AddWithValue("@" + parameter.Key, parameter.Value);
            }

            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                // Обробка результату запиту
            }
            else
            {
                MessageBox.Show("Incorrect data", "Error", MessageBoxButtons.OK);
            }
        }


        public void WriteData(SqlConnection sqlConnection, string tableName, Dictionary<string, object> parameters)
        {
            sqlConnection.Open();

            // Формуємо SQL-запит для вставки даних у відповідну таблицю
            string sqlQuery = "INSERT INTO " + tableName + " (";
            foreach (var parameter in parameters)
            {
                sqlQuery += parameter.Key + ", ";
            }
            sqlQuery = sqlQuery.Remove(sqlQuery.Length - 2); // Видаляємо останню кому
            sqlQuery += ") VALUES (";
            foreach (var parameter in parameters)
            {
                sqlQuery += "@" + parameter.Key + ", ";
            }
            sqlQuery = sqlQuery.Remove(sqlQuery.Length - 2); // Видаляємо останню кому
            sqlQuery += ")";

            SqlCommand command = new SqlCommand(sqlQuery, sqlConnection);

            // Додаємо параметри до команди SQL
            foreach (var parameter in parameters)
            {
                command.Parameters.AddWithValue("@" + parameter.Key, parameter.Value);
            }

            int rowAffected = command.ExecuteNonQuery();

            if (rowAffected > 0)
            {
                MessageBox.Show("Data has been successfully updated");
            }
            else
            {
                MessageBox.Show("Error. Data has not been successfully updated");
            }
        }

    }
}
