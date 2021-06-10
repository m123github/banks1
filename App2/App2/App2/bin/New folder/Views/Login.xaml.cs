using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLite;
using Acr.UserDialogs;

namespace App2
{

    public partial class Login : ContentPage
    {

        private Accountlist Accountlist;
        private SQLiteConnection conn;
        private Customer cust;
        private string details;

        public Login(string BankId1)
        {
            // var image = new Image { Source = "bnk.PNG" };
            InitializeComponent();
            conn = SqlHelper.GetConnection();
            NavigationPage.SetHasNavigationBar(this, false);
            // CustId.Text= "00010000052681";
            BankId.Text = BankId1;
            LoginValues();
            BankId.IsEnabled = false;
            CustId.IsEnabled = false;
            // Pin.Text = "5679";

        }
        public void LoginValues()
        {
            var a = conn.Table<User>().FirstOrDefault();
            if (a != null)
            {
                CustId.Text = a.CustId.Substring(9);
                BranchID.Text = a.BranchID + "/";
                //btnLogin.IsVisible = true;
                //btnRegister.IsVisible = false;
            }
            //else
            //{
            //    btnLogin.IsEnabled = false;
            //    btnRegister.IsVisible = true;
            //}
        }
        async public void LoginCheck()
        {
            //IsPremiumVersion();
            //var abc = conn.Table<User>().FirstOrDefault();
            //var dataExists = true;
            //if (abc == null)
            //{
            //    abc = new User();
            //    dataExists = false;
            //}
            //if (Application.Current.Properties.ContainsKey("BankId"))
            //{
            //    abc.BankId = Application.Current.Properties["BankId"].ToString();
            //}
            //abc.CustId = CustId.Text.ToString();
            //if (dataExists)
            //    conn.Update(abc);
            //else
            //    conn.Insert(abc);
            //var tn = conn.Table<User>().SingleOrDefault();
            Application.Current.Properties["BankId"] = BankId.Text;
            Application.Current.Properties["CustId"] = BranchID.Text.Replace("/", "").Trim() + "00000" + CustId.Text;
            Application.Current.Properties["Pin"] = Pin.Text;
            Accountlist = new Accountlist(BranchID.Text.Replace("/", "").Trim() + "00000" + CustId.Text, BankId.Text);
            var Accounts = Accountlist.Accounts;
            if (Accounts != null)
            {
                conn.CreateTable<Account>();
                conn.DeleteAll<Account>();
                conn.InsertAll(Accounts);
                await Navigation.PushAsync(new Dashboard(cust));
                //App.Current.MainPage = new MainPage();
            }
            else
            {
                await DisplayAlert("Alert", "No Details Available", "Ok");
            }
        }
        private async void NavigateButton_OnClicked_register(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Registration());
        }
        private async void NavigateButton_OnClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(BankId.Text) || string.IsNullOrEmpty(CustId.Text) || string.IsNullOrEmpty(Pin.Text))
            {
                await DisplayAlert("Alert", "Please enter all fields", "Ok");
                return;
            }
            try
            {
                UserDialogs.Instance.ShowLoading("Signing in", MaskType.Gradient);
                await Task.Delay(1000);
                HttpClient client = new HttpClient();
                string l_strUrl = "https://naiveplusmobileappadv.in/api/passbook/Login";
                DataTable l_dtSetupDetailed = new DataTable();
                string l_strCustomerid = BranchID.Text.Replace("/", "").Trim() + "00000" + CustId.Text;
                // string l_strPassword = "1234";
                string l_strPassword = Pin.Text;
                string l_strBankID = BankId.Text;
                JObject l_joSend = new JObject();
                l_joSend.Add("l_strCustomerId", Encryption.EncryptX(l_strCustomerid));
                l_joSend.Add("l_strPassword", Encryption.EncryptX(l_strPassword));
                l_joSend.Add("l_strBankID", Encryption.EncryptX(l_strBankID));
                HttpResponseMessage response = client.PostAsJsonAsync(l_strUrl, l_joSend).Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content;
                    JObject jo = JObject.Parse(content.ReadAsStringAsync().Result.ToString());
                    //JObject jo = JObject.Parse(content.ReadAsStringAsync().Result);
                    if (Encryption.DecryptX(jo["STATUS"].ToString()) == "OK" || Encryption.DecryptX(jo["STATUS"].ToString()) == "SUCCESS")
                    {
                        string l_strResult = Encryption.DecryptX(jo["RESULT"].ToString());
                        details = Encryption.DecryptX(jo["ADDITIONAL_INFO"].ToString());
                        List<string> detailsList = details.Split('|').ToList();
                        List<string> detailsList1 = new List<string>();
                        foreach (var i in detailsList)
                        {
                            string a = i.Substring(i.LastIndexOf('=') + 1).Trim();
                            detailsList1.Add(a);

                        }
                        cust = new Customer { NAME = detailsList1[0], HOUSE = detailsList1[1], STREET = detailsList1[2], CITY = detailsList1[3], MOBILE = detailsList1[4], EMAIL = detailsList1[5] };
                        // await  DisplayAlert("Alert", l_strResult, "Ok");
                        string bankName = Encryption.DecryptX(jo["BANK_NAME"].ToString());
                        Application.Current.Properties.Remove("bankname");
                        Application.Current.Properties.Add("bankname", bankName);
                        LoginCheck();
                    }
                    else
                    {
                        await DisplayAlert("Alert", Encryption.DecryptX(jo["RESULT"].ToString()), "Ok");
                    }
                }
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }
    }

}


