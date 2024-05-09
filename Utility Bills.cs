using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ATM_Winforms
{
    public class Utility_Bills
    {
        public int Id {  get; set; }
        public string User_Name { get; set; }
        public string Company_Name { get; set; }
        public int Amount { get; set; }
        public string Address {  get; set; }
        public string Paid {  get; set; }

        public Utility_Bills(int id,string user_name,string company_name, int amount,string address,string paid)
        {
            Id = id;
            User_Name = user_name;
            Company_Name = company_name;
            Amount = amount;
            Address = address;
            Paid = paid;

        }
    }

    public class GlobalUtility_Bills
    {
        public static List<Utility_Bills> utility_Bills { get; } = new List<Utility_Bills>();

        public static void ClearUtility_Bills()
        {
            utility_Bills.Clear();
        }
    }
}
