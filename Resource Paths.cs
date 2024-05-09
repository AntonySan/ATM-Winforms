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

        public static string Logo_imagine { get; } = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Form imagines", "image_2024-03-22_13-58-24.ico");

        public static string InsertCardForm { get; } = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Form imagines", "InsertCardForm.png");
        public static string LoginForm { get; } = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Form imagines", "LoginForm.png");
        public static string MainForm { get; } = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Form imagines", "MainForm.png");
        public static string CashWithdrawalForm { get; } = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Form imagines", "CashWithdrawalForm.png");
        public static string CharityForm { get; } = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Form imagines", "CharityForm.png");
        public static string FinesForm1 { get; } = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Form imagines", "FinesForm1.png");
        public static string FinesForm2 { get; } = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Form imagines", "FinesForm2.png");
        public static string InternetForm { get; } = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Form imagines", "InternetForm.png");
        public static string PaymentHistoryForm { get; } = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Form imagines", "PaymentHistoryForm.png");
        public static string ReplenishTheCardForm { get; } = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Form imagines", "ReplenishTheCardForm.png");
        public static string TransferByRequisitesForm { get; } = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Form imagines", "TransferByRequisitesForm.png");
        public static string TransferToTheCardForm { get; } = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Form imagines", "TransferToTheCardForm.png");
        public static string UtilityBillsForm { get; } = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Form imagines", "UtilityBillsForm.png");

        public static string DB_connectionString { get; } = "Data Source=.\\sqlexpress;Initial Catalog=ATM;Integrated Security=True";
    }
}
