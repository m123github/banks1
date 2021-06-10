using Acr.UserDialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PassbookView4 : ContentPage
	{
        public ObservableCollection<Passbook1> list1 { get; set; }
        public List<Account> Accounts = new List<Account>();
        public ObservableCollection<Passbook3> displaylist = new ObservableCollection<Passbook3>();
        public List<string> AccountNo = new List<string>();
        public string SelectedAcct = "";
        public string l_strSourceNo = "";
        public string l_strSourceType = "";
        public string l_strSourceID = "";
        private SQLiteConnection conn;
        public string bankName { get; } = "";
        public PassbookView4()
        {
            try
            {

                InitializeComponent();
                Title = "PassBook";
                //NavigationPage.SetHasNavigationBar(this, false);

                list1 = new ObservableCollection<Passbook1>();
                //   conn = DependencyService.Get<ISqlliteInterface>().GetConnection();
                conn = SqlHelper.GetConnection();
                conn.DeleteAll<Passbook1>();

                
                
                //conn.DropTable<Passbook1>();
                // conn.CreateTable<Passbook1>();

                var accounts = (from account in conn.Table<Account>() select account);

                List<string> ls = new List<string>();
                foreach (var acc in accounts)
                {
                    //if (acc.TYPE_DESCR == "Savings Bank")
                    //{
                    ls.Add(acc.AC_NUMBER.ToString());
                    Accounts.Add(acc);
                    //}
                }
                ActPicker.ItemsSource = ls;
                //ListMyAccount();

                bankName = Application.Current.Properties["bankname"].ToString();
                BindingContext = this;

                string[] headerarray = { "Date", "Particulars", "Payment", "Receipt", "Balance" };


                // onload();


                //listView1.ItemsSource = list1;


            }
            catch (Exception ex)
            {

                //   throw;
            }

        }



        public void onload()
        {



            //    var tempAB = Accounts.SingleOrDefault(X => X.AC_NUMBER == SelectedAcct).SOURCE_NO;


            var trans1 = (conn.Table<Passbook1>()/*.Where(X => X.ActNo == tempAB)*/);


            var ty = trans1.Count();
            
            CultureInfo provider = CultureInfo.InvariantCulture;

            var sortedList = new ObservableCollection<Passbook1>(trans1.GroupBy(n => DateTime.ParseExact(n.Date, "dd/mm/yyyy", provider)).SelectMany(i => i).OrderByDescending(y => DateTime.ParseExact(y.Date, "dd/mm/yyyy", provider)));

            foreach (var i in sortedList)
            {
                displaylist.Add(new Passbook3()
                {
                    Balance = i.Balance,
                    CRDR = i.CRDR,
                    //  ID = i.ID,
                    Date = i.Date,
                    Particulars = i.Particulars,
                    Payment = i.Payment,
                    Receipt = i.Receipt,



                });
            }
            //  }
            //int x = 1;
            //foreach (var item in displaylist)
            //{

            //    item.Index = x;
            //    if ((x % 2) == 0)
            //    { item.RowColor = Xamarin.Forms.Color.FromHex("#b7dae5"); }

            //    else
            //    { item.RowColor = Xamarin.Forms.Color.FromHex("#fcfcfc"); }
            //    x++;

            //    //  item.Date = DateTime.ParseExact(item.Date, "yyyy/mm/dd", CultureInfo.InvariantCulture).ToString("dd/mm/yyyy");

            //}




          //  gridView1.ItemsSource = displaylist;

            //listView1.SeparatorColor = Color.Blue;

            // Refresh.IsEnabled = false;

            


        }
        public async void B_Clicked(object sender, EventArgs args)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Loading..", MaskType.Gradient);
                await Task.Delay(1000);

                //   heading.IsVisible = true;

                // Refresh.IsEnabled = false;
                var a = SelectedAcct;
                if (ActPicker.SelectedIndex != -1)
                {
                    GetPassbook();
                    //      Refresh.IsEnabled = true;
                }

                else
                {
                    await DisplayAlert("Alert", "Please select an account", "Ok");
                    //         Refresh.IsEnabled = true;
                }
            }
            finally
            {

                UserDialogs.Instance.HideLoading();
            }
        }
        public void D_Clicked(object sender, EventArgs args)
        {



            conn.DeleteAll<Passbook1>();
            onload();


        }
        public void GetPassbook1()
        {



        }

        private void GetPassbook()
        {



            HttpClient client = new HttpClient();
            string l_strUrl = "https://naiveplusmobileappadv.in/api/passbook/GetPassbook";
            DataTable l_dtSetupDetailed = new DataTable();

            string l_strDate = "";
            string l_strBankID = "";
            if (Application.Current.Properties.ContainsKey("BankId"))
            {
                l_strBankID = Convert.ToString(Application.Current.Properties["BankId"]);
            }

            var tempA = Accounts.SingleOrDefault(X => X.AC_NUMBER == SelectedAcct).SOURCE_NO;


            var trans = (conn.Table<Passbook1>().Where(X => X.ActNo == tempA));


            string dtmax = "";

            List<DateTime> dt = new List<DateTime>();

            if (trans.Count() > 0)
            {
                foreach (var tran in trans)
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    var a = DateTime.ParseExact(tran.Date, "dd/mm/yyyy", provider);
                    dt.Add(a);
                }


                dtmax = dt.Max().AddDays(1).ToString("yyyy/MM/dd");
            }


            //CultureInfo provider1 = CultureInfo.InvariantCulture;

            string mindate = DateTime.Now.AddYears(-20).ToString("yyyy/MM/dd");


            l_strDate = trans.Count() > 0 ? dtmax : mindate;



            JObject l_joSend = new JObject();

            l_joSend.Add("l_strSourceType", Encryption.EncryptX(l_strSourceType));
            l_joSend.Add("l_strSourceID", Encryption.EncryptX(l_strSourceID));
            l_joSend.Add("l_strSourceNo", Encryption.EncryptX(l_strSourceNo));
            l_joSend.Add("l_strDate", Encryption.EncryptX(l_strDate));
            l_joSend.Add("l_strBankID", Encryption.EncryptX(l_strBankID));
            l_joSend.Add("l_strRecentOnlyYN", Encryption.EncryptX("N"));



            try
            {

                HttpResponseMessage response = client.PostAsJsonAsync(l_strUrl, l_joSend).Result;

                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content;
                    JObject jo = JObject.Parse(content.ReadAsStringAsync().Result.ToString());


                    if (Encryption.DecryptX(jo["STATUS"].ToString()) == "OK" || Encryption.DecryptX(jo["STATUS"].ToString()) == "SUCCESS")
                    {
                        JObject jResult = JsonConvert.DeserializeObject<JObject>(Encryption.DecryptX(jo["RESULT"].ToString()));


                        if (jResult["Table1"].Count() > 0)
                        {


                            var passbookData = JsonConvert.DeserializeObject<ObservableCollection<Passbook1>>(jResult["Table1"].ToString());




                            foreach (var p in passbookData)
                            {
                                p.Date = DateTime.ParseExact(p.Date, "dd/mm/yyyy", CultureInfo.InvariantCulture).ToString("dd/mm/yyyy");
                                p.ActNo = l_strSourceNo;
                            }





                            SqlHelper.GetConnection().InsertAllWithChildren(passbookData);
                            onload();

                        }

                        else
                        {
                            onload();

                        }




                    }





                }

                else
                {
                    DisplayAlert("Message", "Unable to Sync with database", "ok");
                    onload();

                }

            }
            catch (Exception e)
            {
                throw e;

                //var trans = (from tran in conn.Table<Passbook1>() select tran);
                //listView1.ItemsSource = trans;
                //throw;

            }
            finally
            {

            }








        }

        private void ActPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActPicker.IsEnabled = false;

            B_Clicked(null, null);
            SelectedAcct = ActPicker.SelectedItem.ToString();
            //Account a= new Account({ (from Accountlist in Accounts where Accountlist.AC_NUMBER == SelectedAcct select Accountlist) });
            l_strSourceNo = Accounts.SingleOrDefault(a => a.AC_NUMBER == SelectedAcct).SOURCE_NO;
            l_strSourceID = Accounts.SingleOrDefault(a => a.AC_NUMBER == SelectedAcct).SOURCE_ID;
            l_strSourceType = Accounts.SingleOrDefault(a => a.AC_NUMBER == SelectedAcct).SOURCE_TYPE;

            ActPicker.IsEnabled = true;
        }

    }
}