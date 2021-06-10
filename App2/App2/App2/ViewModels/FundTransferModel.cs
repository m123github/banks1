using System;
using System.Collections.Generic;
using System.Text;

namespace App2
{
   public class FundTransferModel
    {

        public string ToAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string FromAccountNumber { get; set; }
        public string Remarks { get; set; }
        public string Date { get; set; }
        public string PaymentType { get; set; }

    }
}
