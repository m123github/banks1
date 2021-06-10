using Acr.UserDialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddPayee : ContentPage
	{
        public string CustId;
        public string Pin1;
        public string BankId;
        string Receivedotp;
        string otpsuccess;
        public JArray AccountTypes = new JArray();
        public bool canProceed = false;
        public string bankName { get; } = "";
        public List<AccountTypes13> AccountTypes1 = new List<AccountTypes13>();
        public AddPayee (string CustId)
		{
			InitializeComponent ();
            Title = "Add Beneficiary";
            this.CustId = CustId;
         
            LoadAccountTypes();
            bankName = Application.Current.Properties["bankname"].ToString();
            Pin1= Application.Current.Properties["Pin"].ToString();


            BindingContext = this;
        }

        public async void AddNewPayee(object sender, EventArgs args)
        {

          
            if (string.IsNullOrEmpty(txtPayeeAccountName.Text))
            {
                TxtPayeeAccountNumberrReenter_Unfocused(null, null);

            }




            if (string.IsNullOrEmpty(txtPayeeAccountNumber.Text) || string.IsNullOrEmpty(txtPayeeAccountNumberrReenter.Text))
            {
                await DisplayAlert("Alert", "Please enter all fields", "Ok");
                return;

            }

            if (txtPayeeAccountNumber.Text != txtPayeeAccountNumberrReenter.Text) {
                await DisplayAlert("Alert", "Benificiary AccountNumber mismatch ", "Ok");
                return;

            }
            if (!canProceed)
            {
                await DisplayAlert("Alert", "Account doesn't exist ", "Ok");
                return;
            }

            var a = 0;
            MessagingCenter.Subscribe<otpcheck>(this, "Receiveddata", (value) =>
            {
                
                otpsuccess = value.value1;
                if (otpsuccess == "Ok")
                {
                    a++;
                    if (a < 2) {
                        a++;
                        Procced.IsEnabled = false;
                        AddBeneficary(); }

                   
                }

                Navigation.PushAsync(new FundTransfer(new FundTransferModel()));
            });

            btnSendOTP_Click();
            await Navigation.PushPopupAsync(new PinVerification(Receivedotp));


          



           

           
                }




    async    public void AddBeneficary()
        {


           

                HttpClient client = new HttpClient();
            string l_strUrl = "https://naiveplusmobileappadv.in/api/passbook/AddBeneficiary";



            string l_strCustomerId = CustId;
            string l_strTargetType = AccountTypes1.SingleOrDefault(x => x.MENU_DESCR == ddAccountType.SelectedItem.ToString()).TYPE;
          //  string l_strTargetID = "A00";
            string l_strTargetID=   AccountTypes1.SingleOrDefault(x => x.MENU_DESCR == ddAccountType.SelectedItem.ToString()).ID;
          
            string l_strTargetNo = txtPayeeAccountNumber.Text;


            string l_strBankID = BankId;

            string l_strCustNickName =string.IsNullOrEmpty(txtPayeeNickName.Text)? "" : txtPayeeNickName.Text;
            //string l_strCustNickName = txtPayeeNickName.Text;

            JObject l_joSend = new JObject();

            l_joSend.Add("l_strCustomerId", Encryption.EncryptX(l_strCustomerId));
            l_joSend.Add("l_strTargetType", Encryption.EncryptX(l_strTargetType));
            l_joSend.Add("l_strTargetID", Encryption.EncryptX(l_strTargetID));
            l_joSend.Add("l_strTargetNo", Encryption.EncryptX(l_strTargetNo));
            l_joSend.Add("l_strBankID", Encryption.EncryptX(l_strBankID));
            l_joSend.Add("l_strCustNickName", Encryption.EncryptX(l_strCustNickName));
            HttpResponseMessage response = client.PostAsJsonAsync(l_strUrl, l_joSend).Result;

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;
                JObject jo = JObject.Parse(content.ReadAsStringAsync().Result.ToString());


                if (Encryption.DecryptX(jo["STATUS"].ToString()) == "SUCCESS")
                {
                    string jobjResult = Encryption.DecryptX(jo["RESULT"].ToString());
                 await   DisplayAlert("Alert", jobjResult, "Proceed");
                        Procced.IsEnabled = true;

                    }

                else
                {
                    string jobjResult = Encryption.DecryptX(jo["RESULT"].ToString());
                await    DisplayAlert("Alert", jobjResult, "Proceed");
                        Procced.IsEnabled = true;
                    }
            }

            

        }




        private async void btnSendOTP_Click()
        {
            
            
                HttpClient client = new HttpClient();
                string l_strUrl = "https://naiveplusmobileappadv.in/api/passbook/SendOTP";



                JObject l_joSend = new JObject();

                l_joSend.Add("l_strCustomerId", Encryption.EncryptX(CustId));
                l_joSend.Add("l_strBankID", Encryption.EncryptX(BankId));
                l_joSend.Add("l_strPassword", Encryption.EncryptX(Pin1));




                HttpResponseMessage response = client.PostAsJsonAsync(l_strUrl, l_joSend).Result;

                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content;
                    JObject jo = JObject.Parse(content.ReadAsStringAsync().Result.ToString());


                    if (Encryption.DecryptX(jo["STATUS"].ToString()) == "SUCCESS")
                    {
                        string jResult = Encryption.DecryptX(jo["RESULT"].ToString());
                        Receivedotp = jResult;


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
            


        void LoadAccountTypes()
        {
            HttpClient client = new HttpClient();
            string l_strUrl = "https://naiveplusmobileappadv.in/api/passbook/ListAccounts4AddBeneficiary";
            if (Application.Current.Properties.ContainsKey("BankId"))
            {
                BankId = Convert.ToString(Application.Current.Properties["BankId"]);
            }

            string l_strBankID = BankId;             /*"214";*/

            JObject l_joSend = new JObject();            
            l_joSend.Add("l_strBankID", Encryption.EncryptX(l_strBankID));
            HttpResponseMessage response = client.PostAsJsonAsync(l_strUrl, l_joSend).Result;

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;
                JObject jo = JObject.Parse(content.ReadAsStringAsync().Result.ToString());


                if (Encryption.DecryptX(jo["STATUS"].ToString()) == "SUCCESS")
                {
                  var result  = JsonConvert.DeserializeObject<JObject>(Encryption.DecryptX(jo["RESULT"].ToString()));
                   // AccountTypes = JArray.Parse(result["Table1"].ToString());
                  var  AccountTypes2 = JsonConvert.DeserializeObject<ObservableCollection<AccountTypes13>>(result["Table1"].ToString());
                    foreach (var ITEM in AccountTypes2)
                    {
                       var a = new AccountTypes13 { ID = ITEM.ID, MENU_DESCR = ITEM.MENU_DESCR, TYPE = ITEM.TYPE };
                        AccountTypes1.Add(a);
                    }
                    
                    ddAccountType.ItemsSource = AccountTypes1.Select(x => x.MENU_DESCR).ToList();
                    

                }
            }
        }


        private void TxtPayeeAccountNumberrReenter_Unfocused(object sender, FocusEventArgs e)
        {




            //   if (string.IsNullOrEmpty(ddAccountType.SelectedItem.ToString()))

            if (ddAccountType.SelectedIndex  == -1)
            {
                DisplayAlert("", "Account type cannot be empty", "OK");
                return;
            }

            if (string.IsNullOrEmpty(txtPayeeAccountNumber.Text) || string.IsNullOrEmpty(txtPayeeAccountNumberrReenter.Text))
            {
                DisplayAlert("Alert", "Please enter all fields", "Ok");
                return;

            }

            if (txtPayeeAccountNumber.Text != txtPayeeAccountNumberrReenter.Text)
            {
                 DisplayAlert("Alert", "Benificiary AccountNumber mismatch ", "Ok");
                return;

            }
            HttpClient client = new HttpClient();
            string l_strUrl = "https://naiveplusmobileappadv.in/api/passbook/GetDepositorName";
            string l_strSourceID = "";
            foreach (var item in AccountTypes1)
            {
                if(item.MENU_DESCR.ToString() == ddAccountType.SelectedItem.ToString())
                {
                    l_strSourceID = item.ID.ToString();
                    break;

                }
            }           
            string l_strBankID = BankId;              /*"214";*/
            string l_strSourceNo = txtPayeeAccountNumber.Text;
            JObject l_joSend = new JObject();
            l_joSend.Add("l_strSourceID", Encryption.EncryptX(l_strSourceID));
            l_joSend.Add("l_strSourceNo", Encryption.EncryptX(l_strSourceNo));
            l_joSend.Add("l_strBankID", Encryption.EncryptX(l_strBankID));
            HttpResponseMessage response = client.PostAsJsonAsync(l_strUrl, l_joSend).Result;

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;
                JObject jo = JObject.Parse(content.ReadAsStringAsync().Result.ToString());


                if (Encryption.DecryptX(jo["STATUS"].ToString()) == "SUCCESS")
                {
                    var result = Encryption.DecryptX(jo["RESULT"].ToString());
                    if (!string.IsNullOrEmpty(result))
                    {
                        txtPayeeAccountName.Text = result;
                        canProceed = true;
                    }
                    else
                        canProceed = false;
                }
                else
                    canProceed = false;
            }
        
                }

        private void ddAccountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtPayeeAccountNumber.Text != txtPayeeAccountNumberrReenter.Text)
            {


                if (!string.IsNullOrEmpty(txtPayeeAccountNumber.Text) || (!string.IsNullOrEmpty(txtPayeeAccountNumber.Text)))


                DisplayAlert("Alert", "Benificiary AccountNumber mismatch ", "Ok");
                return;

            }


           // TxtPayeeAccountNumberrReenter_Unfocused(null, null);



        }

        async private void MenuItem1_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Dashboard());
            //   await Navigation.PopAsync();
        }

    }

    public class otpcheck
    {

        public string value1 { get; set; }
    }
}