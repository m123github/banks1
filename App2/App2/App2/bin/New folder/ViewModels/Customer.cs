using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace App2
{
    public class Customer
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string NAME { get; set; }
        public string HOUSE { get; set; }
        public string STREET { get; set; }
        public string CITY { get; set; }
        public string EMAIL { get; set; }
        public string MOBILE { get; set; }
    }
}
