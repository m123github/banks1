using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace App2
{
   public class User
    {
        public string CustId { get; set; }
        public string BankId { get; set; }
        [PrimaryKey, AutoIncrement]
        public int  id { get; set; }

        public string BranchID { get; set; }


    }
}
