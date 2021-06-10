using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace App2
{
    public class Account
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }
        public string SOURCE_TYPE { get; set; }
        public string SOURCE_ID { get; set; }
        public string SOURCE_NO { get; set; }
        public string AC_NUMBER { get; set; }
        public string CUST_NAME { get; set; }
        public string TYPE_DESCR { get; set; }

        public string BALANCE { get; set; }
        public string NO_TRANS { get; set; }
        public string ADDITIONAL_INFO { get; set; }
        public string ImgSrc { get; set; }

        public string  NICK_NAME { get; set; }




    }
   


}
