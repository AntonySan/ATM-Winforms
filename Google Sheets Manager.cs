using System;
using System.Collections.Generic;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Sheets.v4;
namespace ATM_APP
{
    internal class Google_Sheets_Manager
    {
        // Параметри для Google Sheets API
        public static string ApplicationName = "Your Application Name";
        public static string SpreadsheetId = "1H77I8Jr2_YAf-gv1ONEDOWgRzo7WMl3_rMTJKtJwR7o";
        public static string SheetName = "Sheet1";
        public static string JsonFilePath =Resource_Paths.JsonFilePath;
        public static string DestinationPath = Resource_Paths.DataBase_XLSX;
        public static string[] Scopes = { SheetsService.Scope.Spreadsheets, DriveService.Scope.Drive };
         public Google_Drive google_Drive = new Google_Drive();

         public void Example()
        {
            // Отримуємо об'єкт управління аутентифікацією
            GoogleCredential credential = GetCredential(JsonFilePath);

            // Отримуємо об'єкти сервісів Google Sheets і Google Drive
            SheetsService sheetsService = GetSheetsService(credential);
            DriveService driveService = google_Drive.GetDriveService();

            // Готуємо дані для запису в Google Sheets
            var data = new Dictionary<string, object> { { "key1", "Hello" }, { "key2", "World" }, { "key3", "Hi" } };

            // Записуємо дані в Google Sheets
            WriteData(sheetsService, SpreadsheetId, SheetName, data);

            // Завантажуємо файл з Google Drive
            DownloadFile(driveService, SpreadsheetId, DestinationPath);


        }


        // Метод для отримання об'єкта управління аутентифікацією
        public static GoogleCredential GetCredential(string jsonFilePath)
        {
            using (var stream = new FileStream(jsonFilePath, FileMode.Open, FileAccess.Read))
            {
                return GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }
        }

        // Метод для отримання об'єкта сервісу Google Sheets
        public static SheetsService GetSheetsService(GoogleCredential credential)
        {
            return new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
        }

        // Метод для отримання об'єкта сервісу Google Drive
       

        // Метод для запису даних в Google Sheets
        public static void WriteData(SheetsService service, string spreadsheetId, string sheetName, Dictionary<string, object> data)
        {
            var rows = new List<IList<object>>();

            // Додавання заголовків стовпців
            var headerRow = new List<object>();
            foreach (var key in data.Keys)
            {
                headerRow.Add(key);
            }
            rows.Add(headerRow);

            // Додавання значень у відповідні стовпці
            var dataRow = new List<object>();
            foreach (var value in data.Values)
            {
                dataRow.Add(value);
            }
            rows.Add(dataRow);

            // Оновлення даних у таблиці
            var range = $"{sheetName}!A1:{char.ConvertFromUtf32(data.Count + 64)}2";
            var valueRange = new Google.Apis.Sheets.v4.Data.ValueRange { Values = rows };
            var updateRequest = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            var updateResponse = updateRequest.Execute();
        }

        // Метод для завантаження файлу з Google Drive
        public static void DownloadFile(DriveService driveService, string fileId, string destinationPath)
        {
            var request = driveService.Files.Get(fileId);
            var file = request.Execute();

            var outputStream = new System.IO.FileStream(destinationPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            driveService.Files.Export(fileId, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet").Download(outputStream);
            outputStream.Close();

            

        }

    }
}
