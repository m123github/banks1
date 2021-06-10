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
	public partial class otpgeneration : ContentPage
	{
        string Receivedotp = "";
        string Resentotp = "";
        string CustId;
        String Bankid;
        string pin;
        public otpgeneration (string otp,string CustId1, string BankId1, string pin)
		{
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent ();
            CustId = CustId1;
            Bankid = BankId1;
            this.pin = pin;

            Receivedotp = otp;

            
        }

        protected override void OnAppearing()
        {

           DisplayAlert("Alert", "OTP is " + Receivedotp + "", "Ok");

            base.OnAppearing();

        }


                                    




        //vALIDATE otp

        private async void NavigateButton_OnClicked_otp(object sender, EventArgs e)
        {
          if(  string.IsNullOrEmpty(otp.Text))
            
            {

              //  await DisplayAlert("Alert", "Please enter all fields", "Ok");
                return;

            }


           


            if (otp.Text == Receivedotp)
            {
                var currentUser = SqlHelper.GetConnection().Table<User>().FirstOrDefault();
                if(currentUser == null)
                {
                    User newUser = new User();
                    newUser.BankId = Bankid;
                    newUser.CustId = CustId;
                    newUser.BranchID = CustId.Substring(0, 4);
                    SqlHelper.GetConnection().Insert(newUser);
                }
                var answer= await DisplayAlert("Sucessfully Registered !!!", "Do you like to change the pin?","Yes","No");

                if (answer)
                { await Navigation.PushAsync(new pin(CustId,Bankid)); }
                else
                {
                    await Navigation.PushAsync(new Login(Bankid));
                }
            }
            else
            {

                await DisplayAlert("Attention", "You have entered Wrong OTP", "Try Again");
            }

            

        }
        //Resend OTP
        private void Button_Clicked(object sender, EventArgs e)
        {
            SendOTP_Click();
        }

        private void SendOTP_Click()
        {
            HttpClient client = new HttpClient();
            string l_strUrl = "https://naiveplusmobileappadv.in/api/passbook/SendOTP";


            string l_strCustomerId = CustId;                   /*"00010000052681";*/
            string l_strBankID =Bankid;              /*"214";*/
            JObject l_joSend = new JObject();

            l_joSend.Add("l_strCustomerId", Encryption.EncryptX(l_strCustomerId));
            l_joSend.Add("l_strBankID", Encryption.EncryptX(l_strBankID));
            l_joSend.Add("l_strPassword", Encryption.EncryptX(pin));

            HttpResponseMessage response = client.PostAsJsonAsync(l_strUrl, l_joSend).Result;
           // Receivedotp = new Random().Next(9999, 99999).ToString();
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;
                JObject jo = JObject.Parse(content.ReadAsStringAsync().Result.ToString());


                if (Encryption.DecryptX(jo["STATUS"].ToString()) == "SUCCESS")
                {
                    string jResult = Encryption.DecryptX(jo["RESULT"].ToString());
                    Receivedotp = jResult;
                    //DisplayAlert("Alert", "OTP is " + Receivedotp + "", "Ok");

                }
                //DisplayAlert("Alert", "OTP is " + Receivedotp + "", "Ok");

            }
          //  DisplayAlert("Alert", "OTP is " + Receivedotp + "", "Ok");
        }


    }
}