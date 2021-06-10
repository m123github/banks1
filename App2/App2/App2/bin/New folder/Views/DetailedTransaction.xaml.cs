
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailedTransactions : ContentPage
    {
        public ObservableCollection<TransactionModel> transactions { get; set; }
        public DetailedTransactions()
        {
            InitializeComponent();
            Title = "Detailed Transactions";
            GetDetailedStatement();
        }

        public void GetDetailedStatement()
        {
            //DateTime StartDate = Convert.ToDateTime(DateTime.Now.Date.ToShortDateString());
            DateTime StartDate = DateTime.Today.AddMonths(-1);
            DateTime EndDate = DateTime.Today;

            // Button ctrl = ((Button)sender);
            //ctrl.BackgroundColor = Color.LightGray;

            // DateTime StartDate = dtStartDate.Date; DateTime EndDate = dtEndDate.Date;
            var randomTest = new Random();
            transactions = new ObservableCollection<TransactionModel>();
            for (int nCnt = 0; nCnt < 15; nCnt++)
            {
                TimeSpan timeSpan = EndDate - StartDate;

                TimeSpan newSpan = new TimeSpan(randomTest.Next(0, (int)timeSpan.TotalDays), 0, randomTest.Next(0, (int)timeSpan.TotalMinutes), 0);
                DateTime newDate = StartDate + newSpan;

                transactions.Add(new TransactionModel { Amount = randomTest.Next(0, 100000), Date = newDate.ToString("dd/MM/yyyy"), Detail = "By lambdazen", Type = (nCnt / 2 != 0) ? "Cr" : "Dr" });

            }

            foreach (TransactionModel T1 in transactions)
            {
                if (T1.Type == "Cr")
                {
                    T1.LabelColor = Color.FromHex("#28B463");

                }
                else
                {
                    T1.LabelColor = Color.FromHex("#FF3345");
                }


            }
            DateTime d = DateTime.Today;
            string today = d.Day.ToString();


            listView.ItemsSource = transactions.OrderBy(i => i.Date);
        }
    }
    

    
}
//public void GetDetailedStatement(object sender, EventArgs args)
//{
//    //DateTime StartDate = Convert.ToDateTime(DateTime.Now.Date.ToShortDateString());
//    DateTime StartDate = DateTime.Today.AddMonths(-1);
//    DateTime EndDate = DateTime.Today;

//    Button ctrl = ((Button)sender);
//    ctrl.BackgroundColor = Color.LightGray;

//    // DateTime StartDate = dtStartDate.Date; DateTime EndDate = dtEndDate.Date;
//    var randomTest = new Random();
//    transactions = new ObservableCollection<TransactionModel>();
//    for (int nCnt = 0; nCnt < 15; nCnt++)
//    {
//        TimeSpan timeSpan = EndDate - StartDate;

//        TimeSpan newSpan = new TimeSpan(randomTest.Next(0, (int)timeSpan.TotalDays), 0, randomTest.Next(0, (int)timeSpan.TotalMinutes), 0);
//        DateTime newDate = StartDate + newSpan;

//        transactions.Add(new TransactionModel { Amount = randomTest.Next(0, 100000), Date = newDate.ToString("dd/MM/yyyy"), Detail = "By lambdazen", Type = (nCnt / 2 != 0) ? "Cr" : "Dr" });

//    }
//    DateTime d = DateTime.Today;
//    string today = d.Day.ToString();


//    listView.ItemsSource = transactions.OrderBy(i => i.Date);
//}
//    }