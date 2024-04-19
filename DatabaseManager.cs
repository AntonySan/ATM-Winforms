 using System;
using System.Collections.Generic;
//using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data.OleDb;
using Microsoft.Data.SqlClient;
using System.Data.Odbc;
namespace ATM_APP
{
    internal class DatabaseManager
    {
        public static void ConnectToDatabase(Form form)
        {
            string connString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\anton\source\repos\ATM Winforms\Database1.accdb;";

            // Creating an OdbcConnection object to connect to the Access database
            using (OdbcConnection conn = new OdbcConnection(connString))
            {
                try
                {
                    conn.Open();
                    MessageBox.Show("Connected to the database");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to connect to the database: " + ex.Message, "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public static void ReadData(SqlConnection sqlConnection, string tableName, List<string> fields)
        {
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
                    string message = "";
                    foreach (string field in fields)
                    {
                        message += $"{field}: {reader[field].ToString()}\n";
                    }
                    MessageBox.Show(message);
                }
            }
            else
            {
                MessageBox.Show("No data found", "Error", MessageBoxButtons.OK);
            }
        }


        public static void WriteData(SqlConnection sqlConnection, string tableName, Dictionary<string, object> parameters)
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
