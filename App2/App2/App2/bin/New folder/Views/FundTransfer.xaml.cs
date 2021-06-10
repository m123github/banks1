

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FundTransfer : ContentPage
    {

        ObservableCollection<Account> BenificiaryAccounts ;
        ObservableCollection<Account> Accounts;
        string CustId;
        string BankId;
        List<string> Benificiarylist;
        public string bankName { get; } = "";

        public FundTransfer(FundTransferModel TransferDetail)
        {
            InitializeComponent();
            Title = "Fund Transfer";
            TransferDetail.PaymentType = "Later";
            this.BindingContext = TransferDetail;
            BenificiaryAccounts = new ObservableCollection<Account>();
            Benificiarylist = new List<string>();

            if (Application.Current.Properties.ContainsKey("BankId"))
            {
                BankId = Convert.ToString(Application.Current.Properties["BankId"]);


            }
            if (Application.Current.Properties.ContainsKey("CustId")) ;
            {
                CustId = Convert.ToString(Application.Current.Properties["CustId"]);
            }
            bankName = Application.Current.Properties["bankname"].ToString();
            BindingContext = this;

            Accounts = new Accountlist(CustId, BankId).Accounts;
            ddFromAccount.ItemsSource = Accounts.Select(item => item.AC_NUMBER).ToList();                                                                                /*Accounts.Select(item =>   item.AC_NUMBER).ToList();*/

          
        }

        public void AddPayee(object sender, EventArgs args)
        {
            Navigation.PushAsync(new AddPayee(CustId));
        }

        protected override void OnAppearing()
        {
            ListBenificiary();

            ddPayee.ItemsSource = BenificiaryAccounts.Select(item => item.NICK_NAME + "-" + item.SOURCE_NO).ToList();
        }
        public async void RemovePayee(object sender, EventArgs args)
        {
         await  Navigation.PushAsync(new RemovePayee(BenificiaryAccounts,CustId,BankId) );
        }

        public async void ConfirmTransaction(object sender, EventArgs args)
        {
           


            if (string.IsNullOrEmpty(txtAmount.Text) || ddPayee.SelectedIndex == -1 || ddFromAccount.SelectedIndex == -1  || (string.IsNullOrEmpty(pin12.Text)))
            {
                await DisplayAlert("Alert", "Please enter all fields", "Ok");
                return;
            }

            if (pin12.Text != Application.Current.Properties["Pin"].ToString())
            {
                await DisplayAlert("Alert", "Please Enter Correct Pin", "Ok");
                return;

            }


                HttpClient client = new HttpClient();
                string l_strUrl = "https://naiveplusmobileappadv.in/api/passbook/FundTransefr";
             // string l_strTargetID = "A00";
           
           

            // string l_strTargetNo = "0001000008779";

            string i = ddPayee.SelectedItem.ToString();
            string l_strTargetNo = i.Substring(i.LastIndexOf('-') + 1).Trim();
            string l_strTargetID = BenificiaryAccounts.SingleOrDefault(item => item.SOURCE_NO == l_strTargetNo).SOURCE_ID;

            //   string l_strSourceID = "A00";
            string l_strSourceID = Accounts.SingleOrDefault(item => item.AC_NUMBER == ddFromAccount.SelectedItem.ToString()).SOURCE_ID;

            // string l_strSourceNo = "0001000008774";
            string l_strSourceNo = Accounts.SingleOrDefault(item => item.AC_NUMBER == ddFromAccount.SelectedItem.ToString()).SOURCE_NO;

                // double l_dblAmount = 200;
                double l_dblAmount = Convert.ToDouble(txtAmount.Text);

                // string l_strBankID = "214";
                string l_strBankID = BankId;



                JObject l_joSend = new JObject();
                l_joSend.Add("l_strTargetID", Encryption.EncryptX(l_strTargetID));
                l_joSend.Add("l_strTargetNo", Encryption.EncryptX(l_strTargetNo));
                l_joSend.Add("l_strSourceID", Encryption.EncryptX(l_strSourceID));
                l_joSend.Add("l_strSourceNo", Encryption.EncryptX(l_strSourceNo));
                l_joSend.Add("l_dblAmount", Encryption.EncryptX(l_dblAmount.ToString()));
                l_joSend.Add("l_strBankID", Encryption.EncryptX(l_strBankID));

                HttpResponseMessage response = client.PostAsJsonAsync(l_strUrl, l_joSend).Result;

                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content;
                    JObject jo = JObject.Parse(content.ReadAsStringAsync().Result.ToString());


                    if (Encryption.DecryptX(jo["STATUS"].ToString()) == "SUCCESS")
                    {
                        string jobjResult = Encryption.DecryptX(jo["RESULT"].ToString());

                      await  DisplayAlert("Alert", "SuccessFully Transfered", "ok");

                    }

                    else
                    {
                      await  DisplayAlert("Alert", Encryption.DecryptX(jo["RESULT"].ToString()), "Ok");

                    }


                }

                else
                {
                    await DisplayAlert("Alert", "Server Connection Error", "Ok");
                }


            


        }





        private void ListBenificiary()
        {
            HttpClient client = new HttpClient();
            string l_strUrl = "https://naiveplusmobileappadv.in/api/passbook/ListBeneficiary";


            //  string l_strCustomerId = "00010000052681";
           string l_strCustomerId = CustId;
            // string l_strBankID = "214";
            string l_strBankID = BankId;

            JObject l_joSend = new JObject();

            l_joSend.Add("l_strCustomerId", Encryption.EncryptX(l_strCustomerId));
            l_joSend.Add("l_strBankID", Encryption.EncryptX(l_strBankID));

            HttpResponseMessage response = client.PostAsJsonAsync(l_strUrl, l_joSend).Result;

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;
                JObject jo = JObject.Parse(content.ReadAsStringAsync().Result.ToString());


                if (Encryption.DecryptX(jo["STATUS"].ToString()) == "SUCCESS")
                {
                    JObject jResult = JsonConvert.DeserializeObject<JObject>(Encryption.DecryptX(jo["RESULT"].ToString()));


                    if (jResult["Table1"].Count() > 0)
                    {                    
                         BenificiaryAccounts = JsonConvert.DeserializeObject<ObservableCollection<Account>>(jResult["Table1"].ToString());

                    }


                }
            }



        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}