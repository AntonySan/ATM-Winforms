using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Windows.Forms;
namespace ATM_APP
{
    internal class Exchange_Rates
    {
     
      
        public async Task<Dictionary<string, Tuple<string, string, string, double, double, double, double>>>  GettingExchangeRate()
        {
            string apiUrl = $"https://api.privatbank.ua/p24api/exchange_rates?json&date={DateTime.Now.ToShortDateString()}";
            Dictionary<string, Tuple<string, string, string, double, double, double, double>> rates = new Dictionary<string, Tuple<string, string, string, double, double, double, double>>();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();

                    // Десеріалізуємо отриманий JSON в об'єкт
                    ExchangeRates exchangeRates = JsonConvert.DeserializeObject<ExchangeRates>(json);

                    foreach (var rate in exchangeRates.ExchangeRate)
                    {
                        rates[rate.Currency] = new Tuple<string,string, string, double, double, double, double>(exchangeRates.Date, exchangeRates.Bank, exchangeRates.BaseCurrencyLit, rate.SaleRateNB, rate.PurchaseRateNB, rate.salerate, rate.purchaserate);
                    }
                }
                else
                {
                    MessageBox.Show($"Помилка: {response.StatusCode}");
                }
            }
             return rates;
        }

    }

    public class ExchangeRates
    {
        public string Date { get; set; }
        public string Bank { get; set; }
        public int BaseCurrency { get; set; }
        public string BaseCurrencyLit { get; set; }
        public ExchangeRate[] ExchangeRate { get; set; }
    }

    public class ExchangeRate
    {
        public string BaseCurrency { get; set; }
        public string Currency { get; set; }
        public double SaleRateNB { get; set; }
        public double PurchaseRateNB { get; set; }
        public double salerate { get; set; }
        public double purchaserate { get; set; }
    }
}
