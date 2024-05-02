using ATM_Winforms.Design_Forms;
using System;

using System.Windows.Forms;

namespace ATM_APP
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new InsertCardForm());
        }
    }
}
