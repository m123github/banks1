
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensions.Extensions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailedTransactions1 : ContentPage
    {
        public ObservableCollection<TransactionModel> transactions { get; set; }
        public Account Acct { get; set; }
        public string bankName { get; } = "";
        public DetailedTransactions1(Account Acct)
        {
            InitializeComponent();
            Title = "Detailed Transactions";
            this.Acct = Acct;
            GetDetailedStatement();
            bankName = Application.Current.Properties["bankname"].ToString();
            BindingContext = this;
        }

        public void GetDetailedStatement()
        {
            HttpClient client = new HttpClient();
            string l_strUrl = "https://naiveplusmobileappadv.in/api/passbook/GetPassbook";


            string l_strSourceType =Acct.SOURCE_TYPE;
            string l_strSourceID = Acct.SOURCE_ID;
            string l_strSourceNo = Acct.SOURCE_NO;                         /*"0001000008779";*/
            string l_strDate = "2018/12/29";
            string l_strBankID = "214";
            if (Application.Current.Properties.ContainsKey("BankId"))
            {
                l_strBankID = Convert.ToString(Application.Current.Properties["BankId"]);
            }


            
            string l_strRecentOnlyYN = "Y";

           
            JObject l_joSend = new JObject();
            l_joSend.Add("l_strSourceType", Encryption.EncryptX(l_strSourceType));
            l_joSend.Add("l_strSourceID", Encryption.EncryptX(l_strSourceID));
            l_joSend.Add("l_strSourceNo", Encryption.EncryptX(l_strSourceNo));
            l_joSend.Add("l_strBankID", Encryption.EncryptX(l_strBankID));
            l_joSend.Add("l_strDate", Encryption.EncryptX(l_strDate));
           
            l_joSend.Add("l_strRecentOnlyYN", Encryption.EncryptX(l_strRecentOnlyYN));
            HttpResponseMessage response = client.PostAsJsonAsync(l_strUrl, l_joSend).Result;

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;
                JObject jo = JObject.Parse(content.ReadAsStringAsync().Result.ToString());


                if (Encryption.DecryptX(jo["STATUS"].ToString()) == "SUCCESS")
                {
                   // string jResult = Encryption.DecryptX(jo["RESULT"].ToString());
                    JObject jResult1 = JsonConvert.DeserializeObject<JObject>(Encryption.DecryptX(jo["RESULT"].ToString()));

                    if (jResult1["Table1"].Count() > 0)
                    {


                        var tranData = JsonConvert.DeserializeObject<ObservableCollection<Transaction>>(jResult1["Table1"].ToString());




                        foreach (var p in tranData)
                        {


                          p.TrueDate=  DateTime.ParseExact(p.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                            if (p.Receipt != "0.00")
                            {

                                p.Amount = p.Receipt;
                                p.LabelColor = Color.FromHex("#28B463");
                                p.Type = "Cr";

                            }

                            else if(p.Payment != "0.00")
                            {

                                p.Amount = p.Payment;
                                p.LabelColor = Color.FromHex("#FF3345");
                                p.Type = "Dr";


                            }


                           

                            }

                      tranData = new ObservableCollection<Transaction>( tranData.OrderByDescending(i => i.TrueDate));

                        listView.ItemsSource = tranData;




                    }








                    }

                    else
                    {


                    }
                }


                


            }



                                    


            
        }
    }




