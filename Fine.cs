using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ATM_APP
{
    public class Fine
    {
        public int Id { get; set; }
        public string LicensePlates { get; set; }
        public string FineDescription { get; set; }
        public string Date { get; set; }
        public int FineAmount { get; set; }
        public string Paid { get; set; }

        public Fine(int id, string licensePlates, string finedescription, string date, int fineamount, string paid)
        {
            Id = id;
            LicensePlates = licensePlates;
            FineDescription = finedescription;
            Date = date;
            FineAmount = fineamount;
            Paid = paid;    
        }
    }

    public class GlobalFines 
    {
        public static List<Fine> Fines { get; } = new List<Fine>();

        public static void ClearFines()
        {
            Fines.Clear();
        }


        public static void GetAllFines()
        {
            using (SqlConnection connection = new SqlConnection(Resource_Paths.DB_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Fines "; // Витягуємо тільки неоплачені штрафи

                SqlCommand command = new SqlCommand(query, connection);



                SqlDataReader reader = command.ExecuteReader();

                // Читання результатів запиту
                while (reader.Read())
                {
                    int id = (int)reader["Id"]; ;
                    string licensePlates = reader["license_plates"].ToString();
                    string fineDescription = reader["fine"].ToString();
                    string date = reader["date"].ToString();
                    int fineAmount = Convert.ToInt32(reader["fine_amount"]);
                    string paid = reader["paid"].ToString();
                    // Створюємо об'єкт штрафу та додаємо його до списку
                    Fine fine = new Fine(id, licensePlates, fineDescription, date, fineAmount, paid);
                    GlobalFines.Fines.Add(fine);
                }

                reader.Close();
            }
        }
    }


}
