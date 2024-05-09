using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_Winforms
{
    public class Internet
    {
      public int Id { get; set; }
        public string Account_Number { get; set; }
        public int Paid { get; set; }

        public int Transfer_Amount { get; set; }
        public Internet(int id, string account_Number, int paid, int transfer_Amount)
        {
            Id = id;
            Account_Number = account_Number;
            Paid = paid;
            Transfer_Amount = transfer_Amount;
        }
    }

    public static class GlobalInternetData
    {
        public static List<Internet> internet { get; } = new List<Internet>();
        // Метод для очищення списку користувачів
        public static void ClearInternet()
        {
            internet.Clear();
        }
    }

}
