
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountTypes1 : ContentPage
    {

        ObservableCollection<Account> Accounts = new ObservableCollection<Account>();
        private Accountlist Accountlist;
        public string BankId { get; set; }
        public string CustId { get; set; }
        private SQLiteConnection conn;
        String ActName;
        public string bankName { get; } = "";
        public AccountTypes1()
        {
            InitializeComponent();
           
            
            conn = SqlHelper.GetConnection();
            // conn.CreateTable<Account>();
            // ListMyAccount();
            if (Application.Current.Properties.ContainsKey("BankId"))
            {
                this.BankId = Convert.ToString(Application.Current.Properties["BankId"]);
            }

            if (Application.Current.Properties.ContainsKey("CustId"))
            {
                this.CustId = Convert.ToString(Application.Current.Properties["CustId"]);
            }
            bankName = Application.Current.Properties["bankname"].ToString();
            BindingContext = this;
            //  Accountlist = new Accountlist(CustId, BankId);
            //Accounts = Accountlist.Accounts;

            var a = (from account in conn.Table<Account>() select account);

           foreach( var i in a)
            {
                Accounts.Add(new Account()
                { AC_NUMBER=i.AC_NUMBER,ADDITIONAL_INFO=i.ADDITIONAL_INFO,BALANCE=i.BALANCE,CUST_NAME=i.CUST_NAME,ImgSrc=i.ImgSrc,NO_TRANS=i.NO_TRANS,SOURCE_ID=i.SOURCE_ID,SOURCE_NO=i.SOURCE_NO,SOURCE_TYPE=i.SOURCE_TYPE,TYPE_DESCR=i.TYPE_DESCR});


            }



            ActName = Accounts[0].CUST_NAME;




            var ad = Regex.Split(ActName, @"\s+").Where(s => s != string.Empty);
            StringBuilder builder = new StringBuilder();
            foreach (var i in ad)
            {
                builder.Append(i.First().ToString().ToUpper() + String.Join("", i.Skip(1)).ToLower() + " ");

            }
            var ab = builder.ToString();
            ActName = ab;


            listView12.ItemsSource =Accounts;

            Title = "Welcome " + ActName;
        }
    
            //void OnTapGestureRecognizerTapped(object sender, EventArgs args)
            //{

            //    //Navigation.InsertPageBefore(new BranchLocator(), this);
            //    //Navigation.PopAsync();
            //    Navigation.PushAsync(new DetailedTransactions());

            //}

        private void listView12_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
        }
        async private void MenuItem1_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Dashboard());
          //   await Navigation.PopAsync();
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs args)
        {

            var a= ((TappedEventArgs)args).Parameter.ToString();

            var  b =(Accounts.SingleOrDefault(x => x.AC_NUMBER == a));
            Navigation.PushAsync(new DetailedTransactions1(b));

           
        }
        private void onFrameTapped(object sender, EventArgs args)
        {

            var a = ((TappedEventArgs)args).Parameter.ToString();

            var b = (Accounts.SingleOrDefault(x => x.AC_NUMBER == a));
            Navigation.PushPopupAsync(new AccountDetail(b));


        }

       

    }
    }
