using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App2
{
    public class TransactionModel
    {
        public int accountNumber { get; set; }
        public int Amount { get; set; }
        public string Date { get; set; }
        public string Detail { get; set; }

        public String Type { get; set; }

        public Color LabelColor { get; set; }

    }
}
