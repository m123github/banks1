using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App2
{
   
        public class Passbook1
        {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Particulars { get; set; }
           
            public string Date { get; set; }
            
            public string Payment { get; set; }
            public string Receipt { get; set; }
           
            public string Balance { get; set; }
            public string CRDR { get; set; }
            public int Index { get; set; }
           public string ActNo { get; set; }


        
    }
    public class Passbook2
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Particulars { get; set; }

        public string Date { get; set; }

        public string Payment { get; set; }
        public string Receipt { get; set; }

        public string Balance { get; set; }
        public string CRDR { get; set; }
        public int Index { get; set; }
        public Color RowColor { get; set; }



    }

    public class Passbook3    {
       
        public string Particulars { get; set; }

        public string Date { get; set; }

        public string Payment { get; set; }
        public string Receipt { get; set; }

        public string Balance { get; set; }
        public string CRDR { get; set; }
       



    }

    public class Transaction
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Particulars { get; set; }

        public string Date { get; set; }

        public DateTime TrueDate { get; set; }

        public string Payment { get; set; }
        public string Receipt { get; set; }

        public string Amount { get; set; }

        public string Balance { get; set; }
        public string CRDR { get; set; }
        public int Index { get; set; }
        public string ActNo { get; set; }
        public Color LabelColor { get; set; }

        public string Type { get; set; }



    }


}
