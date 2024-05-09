using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Windows.Forms;
namespace ATM_APP
{
    public class CurrencyExchangeRate
    {
        public string Currency { get; set; }
        public string SaleRate { get; set; }
        public string PurchaseRate { get; set; }

        public CurrencyExchangeRate(string currency, string saleRate, string purchaseRate)
        {
            Currency = currency;
            SaleRate = saleRate;
            PurchaseRate = purchaseRate;
        }
    }


    public class ExchangeRateData
    {
        public string Date { get; set; }
        public string Bank { get; set; }
        public string BaseCurrency { get; set; }
        public string BaseCurrencyLit { get; set; }
        public double ExchangeRate { get; set; }
        public string Currency {  get; set; } 
        public double SaleRateNB { get; set; }
        public double PurchaseRateNB { get; set; }
        public double SaleRate { get; set; }
        public double PurchaseRate { get; set; }
    }
    public class ExchangeRates
    {
        public async Task<Dictionary<string, ExchangeRateData>> GettingExchangeRate()
        {
            string apiUrl = $"https://api.privatbank.ua/p24api/exchange_rates?json&date={DateTime.Now.ToShortDateString()}";
            Dictionary<string, ExchangeRateData> rates = new Dictionary<string, ExchangeRateData>();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();

                    // Десеріалізуємо отриманий JSON в об'єкт
                    ExchangeRateResponse exchangeRatesResponse = JsonConvert.DeserializeObject<ExchangeRateResponse>(json);

                    foreach (var rate in exchangeRatesResponse.ExchangeRate)
                    {
                        var rateData = new ExchangeRateData
                        {
                            Date = exchangeRatesResponse.Date,
                            Bank = exchangeRatesResponse.Bank,
                            BaseCurrency = exchangeRatesResponse.BaseCurrency,
                            BaseCurrencyLit = exchangeRatesResponse.BaseCurrencyLit,
                            ExchangeRate = rate.ExchangeRate,
                            Currency = rate.Currency,
                            SaleRateNB = rate.SaleRateNB,
                            PurchaseRateNB = rate.PurchaseRateNB,
                            SaleRate = rate.salerate,
                            PurchaseRate = rate.purchaserate
                        };
                        rates[rate.Currency] = rateData;
                    }
                }
                else
                {
                    // Обробка помилки
                }
            }
            return rates;
        }

    }

    public class ExchangeRateResponse
    {
            public string Date { get; set; }
            public string Bank { get; set; }
            public string BaseCurrency { get; set; }
            public string BaseCurrencyLit { get; set; }
            public List<CurrencyRate> ExchangeRate { get; set; }
        }

     public class CurrencyRate
        {
            public string Currency { get; set; }
            public double SaleRateNB { get; set; }
            public double PurchaseRateNB { get; set; }
            public double salerate { get; set; }
            public double purchaserate { get; set; }

        public double ExchangeRate { get; set; }
    }

    public static class ExchangeRateGlobal
    {
       public static List<CurrencyExchangeRate> ExchangeRate { get; } = new List<CurrencyExchangeRate>();
    }



}
