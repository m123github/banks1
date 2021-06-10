using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registration : ContentPage
    {
        string otp = "";
        public Registration()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
           // custid.Text = "00010000052681";
            //BankId.Text = "214";
           
            //    Application.Current.Properties["pin"] = pin.Text;


        }
        private async void NavigateButton_OnClicked_reg(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(BankId.Text) || string.IsNullOrEmpty(CustId.Text) || string.IsNullOrEmpty(pin.Text))
            {

                await DisplayAlert("Alert", "Please enter all fields", "Ok");
                return;
            }
            //Santhosh
            Application.Current.Properties["BankId"] = BankId.Text;
            Application.Current.Properties["CustId"] = CustId.Text;

            btnSendOTP_Click();       
        }


        private async void btnSendOTP_Click()
        {
            HttpClient client = new HttpClient();
            string l_strUrl = "https://naiveplusmobileappadv.in/api/passbook/SendOTP";


            string l_strCustomerId = CustId.Text;                   /*"00010000052681";*/
            if (l_strCustomerId.Length < 10)
                l_strCustomerId = BranchID.Text + "00000" + l_strCustomerId;
            else
                l_strCustomerId = BranchID.Text + l_strCustomerId;

            string l_strBankID = BankId.Text;              /*"214";*/
            string l_strPassword = pin.Text;
            JObject l_joSend = new JObject();

            l_joSend.Add("l_strCustomerId", Encryption.EncryptX(l_strCustomerId));
            l_joSend.Add("l_strBankID", Encryption.EncryptX(l_strBankID));
            l_joSend.Add("l_strPassword", Encryption.EncryptX(l_strPassword));




            HttpResponseMessage response = client.PostAsJsonAsync(l_strUrl, l_joSend).Result;

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;
                JObject jo = JObject.Parse(content.ReadAsStringAsync().Result.ToString());


                if (Encryption.DecryptX(jo["STATUS"].ToString()) == "SUCCESS")
                {
                    string jResult = Encryption.DecryptX(jo["RESULT"].ToString());
                    otp = jResult;
                    await Navigation.PushAsync(new otpgeneration(otp, l_strCustomerId, BankId.Text, l_strPassword));

                }
                else
                {
                    await DisplayAlert("Alert", "OTP generation failed ,Please check entered details", "Ok");

                }




            }

            else
            {

                await DisplayAlert("Alert", "Error connecting to server", "Ok");
            }


        }





    }
}