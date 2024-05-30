using System;
using System.Management;
using System.IO;
using System.Windows.Forms;
using ATM_Winforms;
using System.Text;

namespace ATM_APP
{
    internal class File_Creator
    {
       public string SSD_serialNumber = GetHardDriveSerialNumber();
        public static string GetHardDriveSerialNumber()
        {
            // Отримання інформації про жорсткий диск
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_DiskDrive");

            // Витягнення серійного номера жорсткого диска
            foreach (ManagementObject wmiObject in searcher.Get())
            {
                return wmiObject["SerialNumber"].ToString();

            }

            return null;
        }
        public bool IsSSDSerialNumberValid(string serialNumber)
        {
            // Ваші умови для перевірки серійного номера SSD
            if (serialNumber == "FDA8N75221120925J   _00000001.")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void DailyReport()
        {
            // Перевірка серійного номера SSD
            if (SSD_serialNumber != null && IsSSDSerialNumberValid(SSD_serialNumber))
            {
                // Генерація імені файлу з поточною датою
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                



                try
                {
                    
                    // Запис у файли
                    WriteUserInformationToFile(Resource_Paths.Users_DailyReport, $"Users database_{currentDate}.txt",ReportType.Daily);
                    WriteUtilityBillsInformationToFile(Resource_Paths.UTilityBils_DailyReport, $"Utility Bills_{currentDate}.txt", ReportType.Daily);
                    WriteTransactionInformationToFile(Resource_Paths.Transaction_DailyReport, $"Transactions_{currentDate}.txt", ReportType.Daily);
                    WriteInternetInformationToFile(Resource_Paths.Internet_DailyReport, $"Internet Info_{currentDate}.txt", ReportType.Daily);
                    WriteFineInformationToFile(Resource_Paths.Fines_DailyReport, $"Fines_{currentDate}.txt", ReportType.Daily);
                    WriteCompanyDetailsToFile(Resource_Paths.CompanyDetails_DailyReport, $"Company Details_{currentDate}.txt", ReportType.Daily);
                    WriteCharityFondsToFile(Resource_Paths.CharityFonds_DailyReport, $"Charity Fonds_{currentDate}.txt", ReportType.Daily);

                    MessageBox.Show("Звіт успішно записаний до файлів");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при записі звіту: " + ex.Message);
                }
            }
        }
        public void MonthlyReport()
        {
            // Перевірка серійного номера SSD
            if (SSD_serialNumber != null && IsSSDSerialNumberValid(SSD_serialNumber))
            {
                // Перевірка, чи сьогодні перший день місяця
                if (DateTime.Now.Day == 1)
                {
                    // Отримання попереднього місяця та відповідного року
                    DateTime previousMonth = DateTime.Now.AddMonths(-1);
                    string previousMonthName = previousMonth.ToString("MMMM", new System.Globalization.CultureInfo("uk-UA"));
                    string previousMonthYear = previousMonth.Year.ToString();

                    try
                    {
                        // Запис у файли
                        WriteUserInformationToFile(Resource_Paths.Users_MonthlyReport, $"User {previousMonthName} {previousMonthYear}.txt", ReportType.Monthly);
                        WriteUtilityBillsInformationToFile(Resource_Paths.UTilityBils_MonthlyReport, $"Utility Bills {previousMonthName} {previousMonthYear}.txt", ReportType.Monthly);
                        WriteTransactionInformationToFile(Resource_Paths.Transaction_MonthlyReport, $"Transactions {previousMonthName} {previousMonthYear}.txt", ReportType.Monthly);
                        WriteInternetInformationToFile(Resource_Paths.Internet_MonthlyReport, $"Internet Info {previousMonthName} {previousMonthYear}.txt", ReportType.Monthly);
                        WriteFineInformationToFile(Resource_Paths.Fines_MonthlyReport, $"Fines {previousMonthName} {previousMonthYear}.txt", ReportType.Monthly);
                        WriteCompanyDetailsToFile(Resource_Paths.CompanyDetails_MonthlyReport, $"Company Details {previousMonthName} {previousMonthYear}.txt", ReportType.Monthly);
                        WriteCharityFondsToFile(Resource_Paths.CharityFonds_MonthlyReport, $"Charity Fonds {previousMonthName} {previousMonthYear}.txt", ReportType.Monthly);

                        MessageBox.Show("Щомісячний звіт успішно записаний до файлів");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка при записі щомісячного звіту: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Щомісячний звіт можна створити тільки першого числа місяця.");
                }
            }
        }


        public static void WriteUserInformationToFile(string directoryPath, string fileName, ReportType reportType)
        {
            try
            {
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                string filePath = Path.Combine(directoryPath, fileName);

                // Генерація імені файлу з поточною датою для щоденного звіту
                if (reportType == ReportType.Monthly && DateTime.Now.Day != 1)
                {
                    Console.WriteLine("Щомісячний звіт буде створений тільки на початку місяця.");
                    return;
                }

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                StringBuilder userInformation = new StringBuilder();

                // Блок з виділеною датою
                userInformation.AppendLine("===================================");
                userInformation.AppendLine($"Date: {currentDate}");
                userInformation.AppendLine("===================================");

                foreach (var user in GlobalData.Users)
                {
                    userInformation.AppendLine($"ID: {user.ID}");
                    userInformation.AppendLine($"Card Number: {user.CardNumber}");
                    userInformation.AppendLine($"Full Name: {user.FullName}");
                    userInformation.AppendLine($"Password: {user.Password}");
                    userInformation.AppendLine($"Expiration Date: {user.ExpirationDate}");
                    userInformation.AppendLine($"Payment System: {user.PaymentSystem}");
                    userInformation.AppendLine($"Balance: {user.Balance}");
                    userInformation.AppendLine($"Address: {user.Address}");
                    userInformation.AppendLine($"Issue Date: {user.IssueDate}");
                    userInformation.AppendLine($"CVV/CVC: {user.CVV_CVC}");
                    userInformation.AppendLine($"Card Status: {user.CardStatus}");
                    userInformation.AppendLine($"Spending Limit: {user.SpendingLimit}");
                    userInformation.AppendLine($"Issuing Bank: {user.IssuingBank}");
                    userInformation.AppendLine($"Card Type: {user.CardType}");
                    userInformation.AppendLine("------------------------------");
                }

                // Перезаписуємо весь файл з новими даними
                File.WriteAllText(filePath, userInformation.ToString());
                Console.WriteLine($"Інформація про користувачів успішно записана у файл: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при записі інформації про користувачів у файл: {ex.Message}");
            }
        }




        public static void WriteUtilityBillsInformationToFile(string directoryPath, string fileName, ReportType reportType)
        {
            try
            {
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                string filePath = Path.Combine(directoryPath, fileName);

                if (reportType == ReportType.Monthly && DateTime.Now.Day != 1)
                {
                    Console.WriteLine("Щомісячний звіт буде створений тільки на початку місяця.");
                    return;
                }

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                StringBuilder utilityBillsInformation = new StringBuilder();

                // Блок з виділеною датою
                utilityBillsInformation.AppendLine("===================================");
                utilityBillsInformation.AppendLine($"Date: {currentDate}");
                utilityBillsInformation.AppendLine("===================================");

                foreach (var bill in GlobalUtility_Bills.utility_Bills)
                {
                    utilityBillsInformation.AppendLine($"ID: {bill.Id}");
                    utilityBillsInformation.AppendLine($"User Name: {bill.User_Name}");
                    utilityBillsInformation.AppendLine($"Company Name: {bill.Company_Name}");
                    utilityBillsInformation.AppendLine($"Amount: {bill.Amount}");
                    utilityBillsInformation.AppendLine($"Address: {bill.Address}");
                    utilityBillsInformation.AppendLine($"Paid: {bill.Paid}");
                    utilityBillsInformation.AppendLine("------------------------------");
                }

                // Перезаписуємо весь файл з новими даними
                File.WriteAllText(filePath, utilityBillsInformation.ToString());
                Console.WriteLine($"Інформація про рахунки успішно записана у файл: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при записі інформації про рахунки у файл: {ex.Message}");
            }
        }



        public static void WriteTransactionInformationToFile(string directoryPath, string fileName, ReportType reportType)
        {
            try
            {
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                string filePath = Path.Combine(directoryPath, fileName);

                if (reportType == ReportType.Monthly && DateTime.Now.Day != 1)
                {
                    Console.WriteLine("Щомісячний звіт буде створений тільки на початку місяця.");
                    return;
                }

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                StringBuilder transactionInformation = new StringBuilder();

                // Блок з виділеною датою
                transactionInformation.AppendLine("===================================");
                transactionInformation.AppendLine($"Date: {currentDate}");
                transactionInformation.AppendLine("===================================");

                foreach (var transaction in GlobalTransactionData.Transactions)
                {
                    transactionInformation.AppendLine($"Transaction ID: {transaction.TransactionId}");
                    transactionInformation.AppendLine($"User ID: {transaction.UserId}");
                    transactionInformation.AppendLine($"Transaction Type: {transaction.TransactionType}");
                    transactionInformation.AppendLine($"Amount: {transaction.Amount}");
                    transactionInformation.AppendLine($"Currency: {transaction.Currency}");
                    transactionInformation.AppendLine($"Timestamp: {transaction.Timestamp}");
                    transactionInformation.AppendLine($"Status: {transaction.Status}");
                    transactionInformation.AppendLine($"Source Account: {transaction.SourceAccount}");
                    transactionInformation.AppendLine($"Destination Account: {transaction.DestinationAccount}");
                    transactionInformation.AppendLine($"Description: {transaction.Description}");
                    transactionInformation.AppendLine("------------------------------");
                }

                // Перезаписуємо весь файл з новими даними
                File.WriteAllText(filePath, transactionInformation.ToString());
                Console.WriteLine($"Інформація про транзакції успішно записана у файл: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при записі інформації про транзакції у файл: {ex.Message}");
            }
        }

        public static void WriteInternetInformationToFile(string directoryPath, string fileName, ReportType reportType)
        {
            try
            {
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                string filePath = Path.Combine(directoryPath, fileName);

                if (reportType == ReportType.Monthly && DateTime.Now.Day != 1)
                {
                    Console.WriteLine("Щомісячний звіт буде створений тільки на початку місяця.");
                    return;
                }

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                StringBuilder internetInformation = new StringBuilder();

                // Блок з виділеною датою
                internetInformation.AppendLine("===================================");
                internetInformation.AppendLine($"Date: {currentDate}");
                internetInformation.AppendLine("===================================");

                foreach (var internet in GlobalInternetData.internet)
                {
                    internetInformation.AppendLine($"ID: {internet.Id}");
                    internetInformation.AppendLine($"Account Number: {internet.Account_Number}");
                    internetInformation.AppendLine($"Address: {internet.Address}");
                    internetInformation.AppendLine($"Paid: {internet.Paid}");
                    internetInformation.AppendLine($"Transfer Amount: {internet.Transfer_Amount}");
                    internetInformation.AppendLine($"Tariff Plan: {internet.Tariff_Plan}");
                    internetInformation.AppendLine($"Payment Date: {internet.Payment_Date}");
                    internetInformation.AppendLine($"Service Status: {internet.Service_Status}");
                    internetInformation.AppendLine($"Data Usage: {internet.Data_Usage}");
                    internetInformation.AppendLine($"User Name: {internet.User_Name}");
                    internetInformation.AppendLine("------------------------------");
                }

                // Перезаписуємо весь файл з новою інформацією
                File.WriteAllText(filePath, internetInformation.ToString());
                Console.WriteLine($"Інформація про інтернет успішно записана у файл: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при записі інформації про інтернет у файл: {ex.Message}");
            }
        }

        public static void WriteFineInformationToFile(string directoryPath, string fileName, ReportType reportType)
        {
            try
            {
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                string filePath = Path.Combine(directoryPath, fileName);

                // Якщо потрібен щомісячний звіт і не перший день місяця, просто повідомте користувача та завершіть функцію
                if (reportType == ReportType.Monthly && DateTime.Now.Day != 1)
                {
                    Console.WriteLine("Щомісячний звіт буде створений тільки на початку місяця.");
                    return;
                }

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                StringBuilder fineInformation = new StringBuilder();

                // Блок з виділеною датою
                fineInformation.AppendLine("===================================");
                fineInformation.AppendLine($"Date: {currentDate}");
                fineInformation.AppendLine("===================================");

                foreach (var fine in GlobalFines.Fines)
                {
                    fineInformation.AppendLine($"ID: {fine.Id}");
                    fineInformation.AppendLine($"License Plates: {fine.LicensePlates}");
                    fineInformation.AppendLine($"Fine Description: {fine.FineDescription}");
                    fineInformation.AppendLine($"Date: {fine.Date}");
                    fineInformation.AppendLine($"Fine Amount: {fine.FineAmount}");
                    fineInformation.AppendLine($"Paid: {fine.Paid}");
                    fineInformation.AppendLine("------------------------------");
                }

                // Перезаписуємо весь файл з новими даними
                File.WriteAllText(filePath, fineInformation.ToString());
                Console.WriteLine($"Інформація про штрафи успішно записана у файл: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при записі інформації про штрафи у файл: {ex.Message}");
            }
        }


        public static void WriteCompanyDetailsToFile(string directoryPath, string fileName, ReportType reportType)
        {
            try
            {
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                string filePath = Path.Combine(directoryPath, fileName);

                if (reportType == ReportType.Monthly && DateTime.Now.Day != 1)
                {
                    Console.WriteLine("Щомісячний звіт буде створений тільки на початку місяця.");
                    return;
                }

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                StringBuilder companyInformation = new StringBuilder();

                // Блок з виділеною датою
                companyInformation.AppendLine("===================================");
                companyInformation.AppendLine($"Date: {currentDate}");
                companyInformation.AppendLine("===================================");

                foreach (var company in CompanyDetails.GlobalCompanyDetails.companies)
                {
                    companyInformation.AppendLine($"ID: {company.Id}");
                    companyInformation.AppendLine($"Company Name: {company.Company_Name}");
                    companyInformation.AppendLine($"IBAN: {company.IBAN}");
                    companyInformation.AppendLine($"Country: {company.Country}");
                    companyInformation.AppendLine($"Address: {company.Address}");
                    companyInformation.AppendLine($"Contact Person: {company.ContactPerson}");
                    companyInformation.AppendLine($"Phone: {company.Phone}");
                    companyInformation.AppendLine($"TIN: {company.TIN}");
                    companyInformation.AppendLine($"EDRPOU: {company.EDRPOU}");
                    companyInformation.AppendLine($"Account Balance: {company.AccountBalance}");
                    companyInformation.AppendLine("------------------------------");
                }

                // Перезаписуємо весь файл з новою інформацією
                File.WriteAllText(filePath, companyInformation.ToString());
                Console.WriteLine($"Інформація про компанії успішно записана у файл: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при записі інформації про компанії у файл: {ex.Message}");
            }
        }


        public static void WriteCharityFondsToFile(string directoryPath, string fileName, ReportType reportType)
        {
            try
            {
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                string filePath = Path.Combine(directoryPath, fileName);

                if (reportType == ReportType.Monthly && DateTime.Now.Day != 1)
                {
                    Console.WriteLine("Щомісячний звіт буде створений тільки на початку місяця.");
                    return;
                }

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                StringBuilder charityInformation = new StringBuilder();

                // Блок з виділеною датою
                charityInformation.AppendLine("===================================");
                charityInformation.AppendLine($"Date: {currentDate}");
                charityInformation.AppendLine("===================================");

                foreach (var charity in GlobalCharityFond.CharityFonds)
                {
                    charityInformation.AppendLine($"ID: {charity.Id}");
                    charityInformation.AppendLine($"Fond Name: {charity.FondName}");
                    charityInformation.AppendLine($"Registration Number: {charity.RegistrationNumber}");
                    charityInformation.AppendLine($"Country: {charity.Country}");
                    charityInformation.AppendLine($"Address: {charity.Address}");
                    charityInformation.AppendLine($"Contact Person: {charity.ContactPerson}");
                    charityInformation.AppendLine($"Phone: {charity.Phone}");
                    charityInformation.AppendLine($"Email: {charity.Email}");
                    charityInformation.AppendLine($"Bank Account: {charity.BankAccount}");
                    charityInformation.AppendLine($"Account Balance: {charity.AccountBalance}");
                    charityInformation.AppendLine("------------------------------");
                }

                // Перезаписуємо весь файл з новими даними
                File.WriteAllText(filePath, charityInformation.ToString());
                Console.WriteLine($"Інформація про благодійні фонди успішно записана у файл: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при записі інформації про благодійні фонди у файл: {ex.Message}");
            }
        }


        public enum ReportType
        {
            Daily,
            Monthly
        }
    }
}
    

