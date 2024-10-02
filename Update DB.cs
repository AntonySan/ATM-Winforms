using ATM_APP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ATM_APP
{
    public class Update_DB 
    {   DatabaseManager databaseManager = new DatabaseManager();
        ExchangeRates exchangeRate = new ExchangeRates();

        public async Task UpdateRates()
        {
            try
            {
                SqlConnection connection = new SqlConnection(Resource_Paths.DB_connectionString);
                // Отримання даних про обмінний курс
                Dictionary<string, ExchangeRateData> rates = await exchangeRate.GettingExchangeRate();

                // Запис даних про обмінний курс в базу даних
                DatabaseManager.WriteData(connection, "Exchange_Rates", rates);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
