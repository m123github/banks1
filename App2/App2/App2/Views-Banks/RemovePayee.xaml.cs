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
	public partial class RemovePayee : ContentPage
	{
        string CustId;
        string BankId;
        public string bankName { get; } = "";
        public ObservableCollection<Account> BenificiaryAccounts { get; set; }
        public RemovePayee(ObservableCollection<Account> BenificiaryAccounts, string CustId1, string BankId1)

        {
            InitializeComponent();

            CustId = CustId1;
            BankId = BankId1;

            Title = "Remove Beneficiary";
            this.BenificiaryAccounts = BenificiaryAccounts;
            var list = BenificiaryAccounts.Select(item => item.NICK_NAME + "-" + item.SOURCE_NO).ToList();
            RemoveAccount.ItemsSource = list;
            bankName = Application.Current.Properties["bankname"].ToString();
            BindingContext = this;

        }



        private void RemoveAccount_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {

            if (RemoveAccount.SelectedIndex == -1)
            {
                await DisplayAlert("Alert", "Please select an account", "Ok");

                return;

            }

            

                HttpClient client = new HttpClient();
                string l_strUrl = "https://naiveplusmobileappadv.in/api/passbook/DelBeneficiary";


                // string l_strCustomerId = "00010000052681";
                string l_strCustomerId = CustId;

                //string l_strBankID = "214";
                string l_strBankID = BankId;
            var accountNo = RemoveAccount.SelectedItem.ToString().Split('-')[1];

            string l_strTargetID = BenificiaryAccounts.FirstOrDefault(x => x.SOURCE_NO == accountNo).SOURCE_ID.ToString();
            //  string l_strTargetNo = "0001000008779";
            string l_strTargetNo = accountNo;

                JObject l_joSend = new JObject();

                l_joSend.Add("l_strCustomerId", Encryption.EncryptX(l_strCustomerId));
                l_joSend.Add("l_strBankID", Encryption.EncryptX(l_strBankID));
                l_joSend.Add("l_strTargetNo", Encryption.EncryptX(l_strTargetNo));
            l_joSend.Add("l_strTargetID", Encryption.EncryptX(l_strTargetID));
            HttpResponseMessage response = client.PostAsJsonAsync(l_strUrl, l_joSend).Result;

                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content;
                    JObject jo = JObject.Parse(content.ReadAsStringAsync().Result.ToString());


                    if (Encryption.DecryptX(jo["STATUS"].ToString()) == "SUCCESS")
                    {
                        string jobjResult = Encryption.DecryptX(jo["RESULT"].ToString());
                      await  DisplayAlert("Alert", jobjResult, "Proceed");

                    }

                    else
                    {
                        string jobjResult = Encryption.DecryptX(jo["RESULT"].ToString());
                       await DisplayAlert("Alert", jobjResult, "Proceed");
                    }

                }

                await Navigation.PushAsync(new FundTransfer(new FundTransferModel()));
            }

        }

        }
