using System;
using System.Management;
using System.IO;
using System.Windows.Forms;
using ATM_Winforms;

namespace ATM_APP
{
    internal class File_Creator
    {
        string SSD_serialNumber = GetHardDriveSerialNumber();
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
                string filePath = Resource_Paths.DailyReport;

                // Ваші дані, які потрібно записати в файл
                string dataToWrite = "Ваші дані";

                try
                {
                    // Запис у файл
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine(dataToWrite);
                    }
                    MessageBox.Show(SSD_serialNumber);
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
            public void MonthlyReport(string folderPath)
        {
            if (IsSSDSerialNumberValid(SSD_serialNumber))
            {
                // Створюємо шлях до теки для файлів
                string monthYearFolder = Path.Combine(folderPath, DateTime.Now.ToString("yyyy-MM"));
                Directory.CreateDirectory(monthYearFolder);

                // Формуємо ім'я файлу на основі місяця та року
                string fileName = Path.Combine(monthYearFolder, DateTime.Now.ToString("MMMM yyyy") + ".txt");

                // Перевіряємо, чи існує файл для поточного місяця та року
                if (File.Exists(fileName))
                {
                    // Читаємо всі рядки з файлу
                    string[] lines = File.ReadAllLines(fileName);

                    // Шукаємо запис з датою в файлі
                    for (int i = 0; i < lines.Length; i++)
                    {
                        // Перевіряємо, чи поточний рядок містить дату
                        if (lines[i].Contains("Дата: " + DateTime.Now.ToString("dd.MM.yyyy")))
                        {
                            // Оновлюємо запис з вказаною датою
                            lines[i] = "Дата: " + DateTime.Now.ToString("dd.MM.yyyy");
                            lines[i + 1] = "Це приклад оновленого тексту для " + DateTime.Now.ToString("MMMM yyyy");
                            lines[i + 2] = "Оновлено: " + DateTime.Now;
                            // Записуємо оновлені рядки назад в файл
                            File.WriteAllLines(fileName, lines);
                            Console.WriteLine("Інформація для " + DateTime.Now.ToString("MMMM yyyy") + " оновлена в файлі: " + fileName);
                            return;
                        }
                    }
                }

                // Якщо запис з вказаною датою не знайдено або файл не існує, додаємо новий запис
                using (StreamWriter streamWriter = new StreamWriter(fileName, true))
                {
                    // Записуємо дані в файл
                    streamWriter.WriteLine("Дата: " + DateTime.Now.ToString("dd.MM.yyyy"));
                    streamWriter.WriteLine("Це приклад тексту, який буде записаний у файл для " + DateTime.Now.ToString("MMMM yyyy"));
                    streamWriter.WriteLine("Це рядок з динамічними даними: " + DateTime.Now);
                }

                Console.WriteLine("Інформація успішно записана в файл для " + DateTime.Now.ToString("MMMM yyyy") + ": " + fileName);
            }
            else
            {

            }
        }
    }
}
