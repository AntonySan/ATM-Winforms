using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_APP
{
    internal class Google_Drive
    {
        

        static string[] Scopes = { DriveService.Scope.Drive };
        static string ApplicationName = "MyApp";
        string JsonFile_Path = Resource_Paths.JsonFilePath;

       public void Example()
        {
            // Автентифікація і створення сервісу Google Drive
            var service = GetDriveService();

            // Перевірка наявності файлу
            string filePath = Resource_Paths.DailyReport;
            if (!File.Exists(filePath))
            {
                // Створення файлу, якщо він відсутній
                CreateFileOnGoogleDrive(service, "example.txt", "Це прикладний текст для заповнення файлу.", "text/plain", "1THHQ1xNOR7dJ6MFPjL9qtXUYaTz3XLLc");

            }
            else
            {
                // Завантаження файлу на Google Диск
                UploadFile(service, filePath, "1THHQ1xNOR7dJ6MFPjL9qtXUYaTz3XLLc");
            }
        }



        public DriveService GetDriveService()
        {
            // Завантаження сертифіката службового облікового запису
            GoogleCredential credential;
            using (var stream = new FileStream(JsonFile_Path, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }

            // Створення сервісу Google Drive API
            return new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        public static void UploadFile(DriveService service, string filePath, string folderId)
        {
            // Отримання інформації про файли у вказаній папці з такою ж назвою
            var listRequest = service.Files.List();
            listRequest.Q = $"'{folderId}' in parents and name='{Path.GetFileName(filePath)}'";
            var files = listRequest.Execute().Files;

            // Якщо знайдено файл з такою ж назвою, оновлення його контенту
            if (files != null && files.Count > 0)
            {
                var fileId = files[0].Id;
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    var updateRequest = service.Files.Update(new Google.Apis.Drive.v3.Data.File(), fileId, stream, "text/plain");
                    updateRequest.Upload();
                }
                Console.WriteLine("Файл оновлено: " + fileId);
            }
            else
            {
                // Створення нового файлу, якщо файл з такою назвою не знайдено
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = Path.GetFileName(filePath),
                    Parents = new List<string> { folderId }
                };
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    var request = service.Files.Create(fileMetadata, stream, "text/plain");
                    request.Upload();
                }
                Console.WriteLine("Файл створено.");
            }
        }

       public static void CreateFileOnGoogleDrive(DriveService service, string fileName, string fileContent, string mimeType, string folderId)
        {
            // Отримання інформації про файли у вказаній папці з такою ж назвою
            var listRequest = service.Files.List();
            listRequest.Q = $"'{folderId}' in parents and name='{fileName}'";
            var files = listRequest.Execute().Files;

            // Якщо знайдено файл з такою ж назвою, оновлення його контенту
            if (files != null && files.Count > 0)
            {
                var fileId = files[0].Id;
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent)))
                {
                    var updateRequest = service.Files.Update(new Google.Apis.Drive.v3.Data.File(), fileId, stream, mimeType);
                    updateRequest.Upload();
                }
                Console.WriteLine("Файл оновлено: " + fileId);
            }
            else
            {
                // Створення нового файлу, якщо файл з такою назвою не знайдено
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = fileName,
                    Parents = new List<string> { folderId }
                };
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent)))
                {
                    var request = service.Files.Create(fileMetadata, stream, mimeType);
                    request.Upload();
                }
                Console.WriteLine("Файл створено.");
            }
        }


    }
}
