using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
//using System.Drawing;
using Xamarin.Forms;


namespace App2
{
   public class Passbook
    {
        public string Date { get; set; }
        public string Chqno { get; set; }
        public string Particulars { get; set; }
        public string Initial { get; set; }
        public string Withdrawals { get; set; }
        public string Deposits { get; set; }
        public string Balance { get; set; }
        public int Index { get; set; }
        public Color RowColor { get; set; }


    }


    public class Passbooklist
    {
        public ObservableCollection<Passbook> List1 { get; set; }


        public Passbooklist()
        {
            List1 = new ObservableCollection<Passbook>  {
            new Passbook { Date = "01/01/2000", Chqno = "67467464", Particulars = "S-444 dep-[811300]", Initial = "", Withdrawals = "2000", Deposits = "234", Balance = "6767262" },
            new Passbook { Date = "01/01/2001", Chqno = "674fff4", Particulars = "Normal-Interest", Initial = "", Withdrawals = "2000", Deposits = "1002", Balance = "347899.0" },
            new Passbook { Date = "01/01/2002", Chqno = "6746etre4", Particulars = "Insta Mojo", Initial = "", Withdrawals = "2000", Deposits = "91", Balance = "9090456"  },
            new Passbook { Date = "01/01/2002", Chqno = "6746ter", Particulars = "to annual Credit", Initial = "", Withdrawals = "2000", Deposits = "12565", Balance = "6766400" },
            new Passbook { Date = "01/01/2001", Chqno = "6746rte4", Particulars = "By APB-BLPG787", Initial = "", Withdrawals = "1000", Deposits = "10000",  Balance = "8230000" },
            };

            int i = 1;
            foreach (Passbook P1 in List1)
            {
                
                P1.Index = i;
                if ((i % 2) == 0)
                    { P1.RowColor = Color.FromHex("#FDFEFE"); }
                
                   else
                    P1.RowColor = Color.FromHex("#D6EAF8");
                i++;
            }

        }

           
        }

        
    }








