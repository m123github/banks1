using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountTypes : ContentPage
	{
        List<Account> Accounts = new List<Account>();

        public AccountTypes ()
		{
			InitializeComponent ();
            ListMyAccount();
            

        }

        private void ListMyAccount()
        {
            HttpClient client = new HttpClient();
            string l_strUrl = "https://naiveplusmobileappadv.in/api/passbook/ListMyAccounts";
            DataTable l_dtSetupDetailed = new DataTable();
            string l_strCustomerid = "00010000052681";
            string l_strBankID = "214";

            JObject l_joSend = new JObject();
            l_joSend.Add("l_strCustomerId", Encryption.EncryptX(l_strCustomerid));
            l_joSend.Add("l_strBankID", Encryption.EncryptX(l_strBankID));

            HttpResponseMessage response = client.PostAsJsonAsync(l_strUrl, l_joSend).Result;

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;
                JObject jo = JObject.Parse(content.ReadAsStringAsync().Result.ToString());

                
                if (Encryption.DecryptX(jo["STATUS"].ToString()) == "OK" || Encryption.DecryptX(jo["STATUS"].ToString()) == "SUCCESS")
                {
                    JObject jResult = JsonConvert.DeserializeObject<JObject>(Encryption.DecryptX(jo["RESULT"].ToString()));
                   

                    for (int i = 0; i < jResult["Table1"].Count(); i++)

                    {
                        Accounts.Add(jResult["Table1"][i].ToObject<Account>());
                    }


                }
                foreach (var i in Accounts)
                {
                   
                }

            }

        }


    }
}