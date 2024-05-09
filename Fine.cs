using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_Winforms
{
    public class Fine
    {
        public int Id { get; set; }
        public string LicensePlates { get; set; }
        public string FineDescription { get; set; }
        public string Date { get; set; }
        public int FineAmount { get; set; }
        public string Paid { get; set; }

        public Fine(int id, string licensePlates, string finedescription, string date, int fineamount, string paid)
        {
            Id = id;
            LicensePlates = licensePlates;
            FineDescription = finedescription;
            Date = date;
            FineAmount = fineamount;
            Paid = paid;    
        }
    }

    public class GlobalFines 
    {
        public static List<Fine> Fines { get; } = new List<Fine>();

        public static void ClearFines()
        {
            Fines.Clear();
        }
    }


}
