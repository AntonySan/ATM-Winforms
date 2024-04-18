using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_Winforms
{
    internal class Resource_Paths
    {
        static string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
        public static string JsonFilePath { get; } = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Credentials", "atm-app-420409-a351e366fb93.json");

        public static string DailyReport { get; } = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,"Project Info", "Daily Report", $"database_{currentDate}.txt");    

        public static string MonthlyReport { get; } = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Project Info", "Monthy Report");

        public static string DataBase_XLSX { get; } = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,  "Project Info", "DataBase XLSX", "daily report.txt");
    }
}
