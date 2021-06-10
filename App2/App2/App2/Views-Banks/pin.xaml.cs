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
	public partial class pin : ContentPage
	{
        string CustId;
        String Bankid;


        public pin (string CustId1,string BankId1)
		{
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent ();
            CustId = CustId1;
            Bankid = BankId1;
            

        }
        private async void NavigateButton_OnClicked_ok(object sender, EventArgs e)
        {

         if (   string.IsNullOrEmpty(pinno.Text) || string.IsNullOrEmpty(pinnum.Text)|| string.IsNullOrEmpty(pinnumber.Text))

           
            {
                await DisplayAlert("Alert", "Please enter all fields", "Ok");
                return;

            }

           
                cmdResetPIN_Click();



                await Navigation.PushAsync(new BankIdLogin());
            
           


        }
        private async void NavigateButton_OnClicked_cancel(object sender, EventArgs e)
        {
            // await Navigation.PushAsync(new BankIdLogin());
            await Navigation.PopAsync();
          
        }

       async private void cmdResetPIN_Click()
        {

           


            HttpClient client = new HttpClient();
            string l_strUrl = "https://naiveplusmobileappadv.in/api/passbook/ResetPIN";


            string l_strCustID = CustId;                          /* "00010000052681";*/
            string l_strOldPIN = pinno.Text;                                 /*   "1234";*/
            string l_strNewPIN = pinnum.Text;                                           /*  "5678";*/
            string l_strConfirmPIN = pinnumber.Text;                                               /*  "5678";*/
            string l_strBankID = Bankid;                      /*"214";*/

            JObject l_joSend = new JObject();
            l_joSend.Add("l_strCustID", Encryption.EncryptX(l_strCustID));
            l_joSend.Add("l_strOldPIN", Encryption.EncryptX(l_strOldPIN));
            l_joSend.Add("l_strNewPIN", Encryption.EncryptX(l_strNewPIN));
            l_joSend.Add("l_strConfirmPIN", Encryption.EncryptX(l_strConfirmPIN));
            l_joSend.Add("l_strBankID", Encryption.EncryptX(l_strBankID));

            HttpResponseMessage response = client.PostAsJsonAsync(l_strUrl, l_joSend).Result;

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;
                JObject jo = JObject.Parse(content.ReadAsStringAsync().Result.ToString());


                if (Encryption.DecryptX(jo["STATUS"].ToString()) == "SUCCESS")
                {
                    string jobjResult = Encryption.DecryptX(jo["RESULT"].ToString());

                   await  DisplayAlert("Alert", "Succesfully Changed Pin", "Ok");

                }

             await   DisplayAlert("Alert", Encryption.DecryptX(jo["RESULT"].ToString()), "Ok");
            }

        }



    }
}