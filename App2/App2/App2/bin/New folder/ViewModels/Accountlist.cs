using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace App2
{


    public class Accountlist
    {
        public ObservableCollection<Account> Accounts { get; set; }
        public string Version { get; set; }
        public string l_strCustomerId { get; set; }
        public string l_strBankID { get; set; }

        private SQLiteConnection conn;


        public Accountlist(string CustId, string BankId)
        {
            this.l_strCustomerId = CustId;
            this.l_strBankID = BankId;
            ListMyAccount();
            IsPremiumVersion();
        }

        private void ListMyAccount()
        {
            HttpClient client = new HttpClient();
            string l_strUrl = "https://naiveplusmobileappadv.in/api/passbook/ListMyAccounts";
            DataTable l_dtSetupDetailed = new DataTable();
           // string l_strCustomerid = "00010000052685";
           // string l_strBankID = "214";

            JObject l_joSend = new JObject();
            l_joSend.Add("l_strCustomerId", Encryption.EncryptX(l_strCustomerId));
            l_joSend.Add("l_strBankID", Encryption.EncryptX(l_strBankID));

            HttpResponseMessage response = client.PostAsJsonAsync(l_strUrl, l_joSend).Result;

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;
                JObject jo = JObject.Parse(content.ReadAsStringAsync().Result.ToString());


                if (Encryption.DecryptX(jo["STATUS"].ToString()) == "OK" || Encryption.DecryptX(jo["STATUS"].ToString()) == "SUCCESS")
                {
                    JObject jResult = JsonConvert.DeserializeObject<JObject>(Encryption.DecryptX(jo["RESULT"].ToString()));


                    //for (int i = 0; i < jResult["Table1"].Count(); i++)

                    //{
                    //    Accounts.Add(jResult["Table1"][i].ToObject<Account>());
                    //}

                    if (jResult["Table1"].Count() > 0)
                    {

                        Accounts = JsonConvert.DeserializeObject<ObservableCollection<Account>>(jResult["Table1"].ToString());


                        foreach (var i in Accounts)
                        {

                            if (i.TYPE_DESCR == "Savings Bank")
                            {
                                i.ImgSrc = "App2.images.bnkaccnt.PNG";

                            }
                            else if (i.TYPE_DESCR == "A Class Shares")
                                i.ImgSrc = "App2.images.share.png";
                            else if (i.TYPE_DESCR == "Gold Loan")
                                i.ImgSrc = "App2.images.GoldLoan.png";
                            else if (i.TYPE_DESCR == "C S O L")
                                i.ImgSrc = "App2.images.CSOL.png";
                            else if (i.TYPE_DESCR == "Fixed Deposit")
                                i.ImgSrc = "App2.images.fixedpst.png";
                            else if (i.TYPE_DESCR == "Commodity Loan")
                                i.ImgSrc = "App2.images.CSOL.png";
                            else if (i.TYPE_DESCR == "Chitty")
                                i.ImgSrc = "App2.images.CSOL.png";
                            else if (i.TYPE_DESCR == "Gold Loan")
                                i.ImgSrc = "App2.images.GoldLoan.png";

                            else if (i.TYPE_DESCR == "Depsoit Loan")
                                i.ImgSrc = "App2.images.deposit_loan.png";



                        }
                        //conn.DeleteAll<Account>();

                        //conn.InsertAllWithChildren(Accounts);

                        //input.First().ToString().ToUpper() + String.Join("", input.Skip(1)



                    }





                }

            }
        }

        private void IsPremiumVersion()
        {
            HttpClient client = new HttpClient();
            string l_strUrl = "https://naiveplusmobileappadv.in/api/passbook/IsPremiumVersion";
            DataTable l_dtSetupDetailed = new DataTable();
           
           // string l_strBankID = "214";

            JObject l_joSend = new JObject();
            
            l_joSend.Add("l_strBankID", Encryption.EncryptX(l_strBankID));

            HttpResponseMessage response = client.PostAsJsonAsync(l_strUrl, l_joSend).Result;

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;
                JObject jo = JObject.Parse(content.ReadAsStringAsync().Result.ToString());

                string a = Encryption.DecryptX(jo["STATUS"].ToString());
                

                    Version = a.ToLower();

                    //input.First().ToString().ToUpper() + String.Join("", input.Skip(1)


                }





            }

        }



    }

