using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_APP
{
    public class Resource_Paths
    {
        static string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
        private static readonly string currentYear = DateTime.Now.Year.ToString();

        // *****************************************************************
        //                          JSON File Path
        // *****************************************************************
        public static string JsonFilePath { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Credentials",
            "atm-app-420409-a351e366fb93.json");

        // *****************************************************************
        //                          Daily Report Paths
        // *****************************************************************
        public static string DailyReport { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Project Info",
            "Daily Report",
            $"database_{currentDate}.txt");

        public static string Users_DailyReport { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Project Info",
            "Daily Report",
            "Users");

        public static string UTilityBils_DailyReport { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Project Info",
            "Daily Report",
            "UTilityBils");

        public static string Transaction_DailyReport { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Project Info",
            "Daily Report",
            "Transactiont");

        public static string Internet_DailyReport { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Project Info",
            "Daily Report",
            "Internet");

        public static string Fines_DailyReport { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Project Info",
            "Daily Report",
            "Fines");

        public static string CompanyDetails_DailyReport { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Project Info",
            "Daily Report",
            "Company Details");

        public static string CharityFonds_DailyReport { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Project Info",
            "Daily Report",
            "CharityFonds");

        // *****************************************************************
        //                          Monthly Report Paths
        // *****************************************************************
        public static string MonthlyReport { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Project Info",
            "Monthy Report");

        public static string Users_MonthlyReport { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Project Info",
            "Monthly Report",
            currentYear);

        public static string UTilityBils_MonthlyReport { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Project Info",
            "Monthly Report",
            currentYear);

        public static string Transaction_MonthlyReport { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Project Info",
            "Monthly Report",
            currentYear);

        public static string Internet_MonthlyReport { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Project Info",
            "Monthly Report",
            currentYear);

        public static string Fines_MonthlyReport { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Project Info",
            "Monthly Report",
            currentYear);

        public static string CompanyDetails_MonthlyReport { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Project Info",
            "Monthly Report",
            currentYear);

        public static string CharityFonds_MonthlyReport { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Project Info",
            "Monthly Report",
            currentYear);

        // *****************************************************************
        //                          Database XLSX Path
        // *****************************************************************
        public static string DataBase_XLSX { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Project Info",
            "DataBase XLSX",
            "daily report.xlsx");

        // *****************************************************************
        //                          Form Images Paths
        // *****************************************************************
        public static string Logo_imagine { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Form imagines",
            "image_2024-03-22_13-58-24.ico");

        public static string InsertCardForm { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Form imagines",
            "InsertCardForm.png");

        public static string LoginForm { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Form imagines",
            "LoginForm.png");

        public static string MainForm { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Form imagines",
            "MainForm.png");

        public static string CashWithdrawalForm { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Form imagines",
            "CashWithdrawalForm.png");

        public static string CharityForm { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Form imagines",
            "CharityForm.png");

        public static string FinesForm1 { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Form imagines",
            "FinesForm1.png");

        public static string FinesForm2 { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Form imagines",
            "FinesForm2.png");

        public static string InternetForm { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Form imagines",
            "InternetForm.png");

        public static string PaymentHistoryForm { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Form imagines",
            "PaymentHistoryForm.png");

        public static string ReplenishTheCardForm { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Form imagines",
            "ReplenishTheCardForm.png");

        public static string TransferByRequisitesForm { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Form imagines",
            "TransferByRequisitesForm.png");

        public static string TransferToTheCardForm { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Form imagines",
            "TransferToTheCardForm.png");

        public static string UtilityBillsForm { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Form imagines",
            "UtilityBillsForm.png");

        // *****************************************************************
        //                   Supplementary Form Images Paths
        // *****************************************************************
        public static string MainSupForm { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Form imagines",
            "AuraBankSup1.png");

        public static string CardSupForm { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Form imagines",
            "AuraBankSup2.png");

        public static string CharitySupForm { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Form imagines",
            "AuraBankSup3.png");

        public static string ComanySupForm { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Form imagines",
            "AuraBankSup4.png");

        public static string InternetSupForm { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Form imagines",
            "AuraBankSup5.png");

        public static string UtilityBillsSupForm { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Form imagines",
            "AuraBankSup6.png");

        public static string FinesSupForm { get; } = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Form imagines",
            "AuraBankSup7.png");

        // *****************************************************************
        //                         Database Connection String
        // *****************************************************************
        public static string DB_connectionString { get; } = "Data Source=.\\sqlexpress;Initial Catalog=ATM;Integrated Security=True";
    }
}
