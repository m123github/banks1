using Acr.UserDialogs;
using HorizontalList;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PassbookView2 : ContentPage
    {
        public ObservableCollection<Passbook1> list1 { get; set; }
       public List<Account> Accounts = new List<Account>();
      public  List<string> AccountNo = new List<string>();
        public string SelectedAcct = "";
        public string l_strSourceNo = "";
        public string l_strSourceType = "";
        public string l_strSourceID = "";
        private SQLiteConnection conn;
        public string bankName { get; } = "";
        public static int rowCounter = 0;
        public PassbookView2()
        {
            try
            {

            InitializeComponent();
            Title = "PassBook";
            //NavigationPage.SetHasNavigationBar(this, false);

            list1 = new ObservableCollection<Passbook1>();
            //   conn = DependencyService.Get<ISqlliteInterface>().GetConnection();
            conn = SqlHelper.GetConnection();
            conn.DeleteAll<Passbook1>();
           //conn.DropTable<Passbook1>();
           // conn.CreateTable<Passbook1>();

             var accounts = (from account in conn.Table<Account>()  select account );

            List<string> ls = new List<string>();
            foreach (var acc in accounts)
            {
                //if (acc.TYPE_DESCR == "Savings Bank")
                //{
                    ls.Add(acc.AC_NUMBER.ToString());
                    Accounts.Add(acc);
                //}
            }
            ActPicker.ItemsSource = ls;
                //ListMyAccount();

                bankName = Application.Current.Properties["bankname"].ToString();
                BindingContext = this;


                // onload();


                //listView1.ItemsSource = list1;


            }
            catch (Exception ex)
            {

             //   throw;
            }

        }



        public void onload()
        {



            var tempAB = Accounts.SingleOrDefault(X => X.AC_NUMBER == SelectedAcct).SOURCE_NO;


            var trans1 = (conn.Table<Passbook1>().Where(X => X.ActNo == tempAB));


            var ty = trans1.Count();
          var displaylist = new ObservableCollection<Passbook2>();
            CultureInfo provider = CultureInfo.InvariantCulture;
            
          var  sortedList = new ObservableCollection<Passbook1>(trans1.GroupBy(n => DateTime.ParseExact(n.Date, "dd/mm/yyyy", provider)).SelectMany(i => i).OrderByDescending(y => DateTime.ParseExact(y.Date, "dd/mm/yyyy", provider)));

            foreach (var i in sortedList)
            {
                displaylist.Add(new Passbook2()
                {
                    Balance = i.Balance,
                    CRDR = i.CRDR,
                    ID = i.ID,
                    Date = i.Date,
                    Particulars = i.Particulars,
                    Payment = i.Payment,
                    Receipt = i.Receipt,
                    


                });
            }
            int x = 1;
            foreach (var item in displaylist)
            {

                item.Index = x;
                if ((x % 2) == 0)
                { item.RowColor = Xamarin.Forms.Color.FromHex("#b7dae5"); }

                else
                { item.RowColor = Xamarin.Forms.Color.FromHex("#fcfcfc"); }
                x++;

              //  item.Date = DateTime.ParseExact(item.Date, "yyyy/mm/dd", CultureInfo.InvariantCulture).ToString("dd/mm/yyyy");

            }



            //  displaylist.OrderByDescending(i => i.Date);

          //  listView1.ItemsSource = displaylist;
            //listView1.SeparatorColor = Color.Blue;

            // Refresh.IsEnabled = false;




        }
       public async void B_Clicked(object sender, EventArgs args)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Loading..", MaskType.Gradient);
                await Task.Delay(1000);

                //heading.IsVisible = true;

               // Refresh.IsEnabled = false;
                var a = SelectedAcct;
                if (ActPicker.SelectedIndex != -1)
                {
                    GetPassbook();
              //      Refresh.IsEnabled = true;
                }

                else
                {
                   await DisplayAlert("Alert", "Please select an account", "Ok");
           //         Refresh.IsEnabled = true;
                }
            }
            finally
            {

                UserDialogs.Instance.HideLoading();
            }
        }
        public void D_Clicked(object sender, EventArgs args)
        {


           
            conn.DeleteAll<Passbook1>();
            onload();
         

        }
        public void GetPassbook1()
        {



        }

       private void GetPassbook()
        {

           

            HttpClient client = new HttpClient();
            string l_strUrl = "https://naiveplusmobileappadv.in/api/passbook/GetPassbook";
            DataTable l_dtSetupDetailed = new DataTable();
           
            string l_strDate = "";
            string l_strBankID = "214";
            if (Application.Current.Properties.ContainsKey("BankId"))
            {
                l_strBankID = Convert.ToString(Application.Current.Properties["BankId"]);
            }

           var tempA=  Accounts.SingleOrDefault(X => X.AC_NUMBER == SelectedAcct).SOURCE_NO;
           

            var trans = (conn.Table<Passbook1>().Where(X => X.ActNo == tempA));

          
            string dtmax="";

                List<DateTime> dt = new List<DateTime>();
            
            if (trans.Count()>0)
            { 
                foreach (var tran in trans)
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    var a = DateTime.ParseExact(tran.Date, "dd/mm/yyyy", provider);
                    dt.Add(a);
                }

                
                dtmax = dt.Max().AddDays(1).ToString("yyyy/MM/dd");
            }


            //CultureInfo provider1 = CultureInfo.InvariantCulture;
           
            string  mindate = DateTime.Now.AddYears(-20).ToString("yyyy/MM/dd");


            l_strDate =  trans.Count() > 0 ?dtmax :mindate;


            
            JObject l_joSend = new JObject();

            l_joSend.Add("l_strSourceType", Encryption.EncryptX(l_strSourceType));
            l_joSend.Add("l_strSourceID", Encryption.EncryptX(l_strSourceID));
            l_joSend.Add("l_strSourceNo", Encryption.EncryptX(l_strSourceNo));
            l_joSend.Add("l_strDate", Encryption.EncryptX(l_strDate));
            l_joSend.Add("l_strBankID", Encryption.EncryptX(l_strBankID));
            l_joSend.Add("l_strRecentOnlyYN", Encryption.EncryptX("N"));




            try
            {
               
                    HttpResponseMessage response =  client.PostAsJsonAsync(l_strUrl, l_joSend).Result;
               
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content;
                    JObject jo = JObject.Parse(content.ReadAsStringAsync().Result.ToString());


                    if (Encryption.DecryptX(jo["STATUS"].ToString()) == "OK" || Encryption.DecryptX(jo["STATUS"].ToString()) == "SUCCESS")
                    {
                        JObject jResult = JsonConvert.DeserializeObject<JObject>(Encryption.DecryptX(jo["RESULT"].ToString()));
                        var align = Encryption.DecryptX(jo["ALIGN"].ToString());
                        if (!string.IsNullOrEmpty(align))
                        {
                            Application.Current.Properties["align"] = align;
                        }
                        string[] align1 = align.Split(',');

                        if (jResult["Table1"].Count() > 0)
                        {


                            //   var passbookData = JsonConvert.DeserializeObject<ObservableCollection<Passbook1>>(jResult["Table1"].ToString());
                            var passbookData1 = JsonConvert.DeserializeObject<JArray>(jResult["Table1"].ToString());
                            var passbookData = passbookData1.ToObject<List<JObject>>();

                            var jobj = passbookData[0];
                            var count = jobj.Children().Count();
                            var colWidth = 10 / count;
                            // heading.Children.ToList().Clear();
                            //heading.ColumnDefinitions.Clear();
                          
                            var heading = new Grid() { BackgroundColor = Color.FromHex("304ea3"), RowSpacing = 10, Margin = new Thickness(0, 2, 0, 1) /*, Padding = new Thickness(10, 0, 0, 0)*/ };
                            var grdList = new Grid() { RowSpacing = 0,ColumnSpacing=0, Margin = new Thickness(0, 2, 0, 1)/*, Padding = new Thickness(10, 0, 0, 0)*/ };
                            heading.RowDefinitions.Add(new RowDefinition { Height = 50 });
                            grdList.RowDefinitions.Add(new RowDefinition { Height = 50 });

                            int i2 = 0;
                            foreach (var key in jobj.Properties())
                            {
                                if (count <= 5)
                                {
                                    layoutList.Orientation = ScrollOrientation.Vertical;
                                    grdList.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(colWidth, GridUnitType.Star) });
                                    
                                }
                                // grdList.ColumnDefinitions.Add(new ColumnDefinition { Width = 70 });
                                else
                                {
                                    layoutList.Orientation = ScrollOrientation.Both;
                                    grdList.ColumnDefinitions.Add(new ColumnDefinition { Width = 70 });
                                }
                                Label lbl = new Label() {BackgroundColor = Color.FromHex("304ea3"), Text = key.Name,FontSize=12,VerticalOptions=LayoutOptions.FillAndExpand, TextColor= Color.FromHex("ffffff"), VerticalTextAlignment=TextAlignment.Center, HorizontalTextAlignment = align1[i2] == "L" ? TextAlignment.Start : TextAlignment.End};
                                if (i2 == 0)
                                {
                                   lbl.HorizontalTextAlignment = TextAlignment.Center;
                                }
                               
                                if(lbl.Text.ToLower() == "descr")
                                {
                                    lbl.Text = "Description";
                                }
                                
                                //if (i2 == 1)
                                //{
                                //    lbl.HorizontalTextAlignment = TextAlignment.Center;
                                //}

                                //if (i2 == jobj.Children().Count() - 1)
                                //{
                                //    lbl.Margin = new Thickness(0, 0, 5, 0);
                                //}
                               


                                grdList.Children.Add(lbl, grdList.ColumnDefinitions.Count - 1, 0);
                                i2++;
                            }
                            CultureInfo provider = CultureInfo.InvariantCulture;
                            var sortedList = new List<JObject>(passbookData.GroupBy(n => DateTime.ParseExact(n["Date"].ToString(), "dd/mm/yyyy", provider)).SelectMany(i => i).OrderByDescending(y => DateTime.ParseExact(y["Date"].ToString(), "dd/mm/yyyy", provider)));

                            rowCounter = 0;
                           

                              int colCounter = 0;
                           // foreach (var key in obj.Properties())
                            for(int ncolcnt = 0; ncolcnt < sortedList.Count; ncolcnt++)
                            {
                                //    if (colCounter == -1)
                                grdList.RowDefinitions.Add(new RowDefinition { Height = 40 });
                                colCounter = 0;
                                var obj = sortedList[ncolcnt];
                                var color = Color.White;
                                if (ncolcnt % 2 == 0)
                                { color = Xamarin.Forms.Color.FromHex("#fcfcfc"); }

                                else
                                { color = Xamarin.Forms.Color.FromHex("#b7dae5"); }


                                int i1 = 0;
                                foreach (var key in obj.Properties())
                                {
                                      ScrollView sv = new ScrollView() { BackgroundColor = color, Orientation = ScrollOrientation.Horizontal, HorizontalScrollBarVisibility = ScrollBarVisibility.Never, VerticalOptions=LayoutOptions.FillAndExpand };
                                    Label lbl = new Label() {BackgroundColor = Color.Transparent, Text = obj[key.Name].ToString(), FontSize =10, TextColor = Color.Black, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = align1[i1] == "L" ? TextAlignment.Start : TextAlignment.End ,LineBreakMode=LineBreakMode.WordWrap};

                                    if (i1 == 0)
                                    {
                                        lbl.HorizontalTextAlignment = TextAlignment.Center;
                                    }


                                    sv.Content = lbl;
                                    //  lbl.SetBinding(Label.TextProperty, key.Name.ToString());
                                    grdList.Children.Add(sv, colCounter, grdList.RowDefinitions.Count - 1);
                                    colCounter++;
                                    i1++;
                                }
                            }

                            ListView lst = new ListView()
                            {
                                Header = heading,
                                ItemTemplate = new DataTemplate(typeof(CustomCell)),
                                ItemsSource = sortedList,
                                MinimumWidthRequest = 1000,
                                Margin = new Thickness(10, 2, 10, 2),
                                VerticalOptions = LayoutOptions.FillAndExpand,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                Rotation = 270
                                //ListOrientation = StackOrientation.Horizontal
                            };
                            ContentView cv = new ContentView();
                            cv.Rotation = 90;
                            //  cv.Content = lst;
                            // listView1.ItemsSource = passbookData;
                            //listView1.HeaderTemplate
                            //listView1.ItemTemplate = myDataTemplate;
                            layoutList.Content = null;
                            layoutList.Content = grdList;
                           //   layoutList.Children.Clear();
                            //layoutList.Children.Add(cv);
                            //foreach (var p in passbookData)
                            //{
                            //    p.Date=  DateTime.ParseExact(p.Date, "dd/mm/yyyy",CultureInfo.InvariantCulture).ToString("dd/mm/yyyy");
                            //    p.ActNo = l_strSourceNo;
                            //}

                            //  listView1.ItemsSource = null;



                            //  SqlHelper.GetConnection().InsertAllWithChildren(passbookData);
                             // onload();

                        }

                        else
                        {
                            onload();

                        }
                    }
                }

                else
                {
                    DisplayAlert("Message", "Unable to Sync with database", "ok");
                    onload();

                }

            }
            catch (Exception e)
            {
               // layoutList.Children.Clear();
                Label lbl = new Label() { Text = "No Data Found", FontSize = 18, TextColor = Color.Black, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center };
                layoutList.Content = lbl;
                // throw e;

                //var trans = (from tran in conn.Table<Passbook1>() select tran);
                //listView1.ItemsSource = trans;
                //throw;

            }
            finally
            {
               
            }








        }

        private void ActPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActPicker.IsEnabled = false;

             B_Clicked(null, null);
            SelectedAcct = ActPicker.SelectedItem.ToString();
            //Account a= new Account({ (from Accountlist in Accounts where Accountlist.AC_NUMBER == SelectedAcct select Accountlist) });
            l_strSourceNo = Accounts.SingleOrDefault(a => a.AC_NUMBER == SelectedAcct).SOURCE_NO;
            l_strSourceID = Accounts.SingleOrDefault(a => a.AC_NUMBER == SelectedAcct).SOURCE_ID;
            l_strSourceType = Accounts.SingleOrDefault(a => a.AC_NUMBER == SelectedAcct).SOURCE_TYPE;

            ActPicker.IsEnabled = true;
        }

      async  private void MenuItem1_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Dashboard());
         //  await Navigation.PopAsync();
        }
    }

    public class CustomCell : ViewCell
    {
        // JObject nameLabel;
        
        Grid grid = new Grid() { RowSpacing = 10, Margin = new Thickness(0, 2, 0, 2), VerticalOptions = LayoutOptions.FillAndExpand };
        public CustomCell()
        {

            //  return cell;
          //  var cell = new ViewCell();
         
            View = grid;

        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                var obj =  (JObject)BindingContext;
              //  var cell = new ViewCell();
                //var grid = new Grid() { RowSpacing = 10, Margin = new Thickness(0, 2, 0, 2), VerticalOptions = LayoutOptions.FillAndExpand };
                // var abcd = this.BindingContext;
                var colCounter = 0;
                var colWidth = 10 / obj.Properties().Count();
                foreach (var key in obj.Properties())
                {
                    if (obj.Properties().Count() <= 5)
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(colWidth, GridUnitType.Star) });
                    else
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 70 });
                }
                //foreach (var obj in passbookData)
                //  {
                

                grid.RowDefinitions.Add(new RowDefinition { Height = 30});
                int i4 = 0;
                //  colCounter = 0;
                foreach (var key in obj.Properties())
                //for(int ncolcnt = 0; ncolcnt < count;ncolcnt++)
                {
                    //    if (colCounter == -1)

                  //  ScrollView sv = new ScrollView() { Orientation = ScrollOrientation.Horizontal, HorizontalScrollBarVisibility = ScrollBarVisibility.Never, VerticalOptions=LayoutOptions.CenterAndExpand };
                    Label lbl = new Label() { Text = obj[key.Name].ToString(), FontSize = 11, TextColor = Color.Black, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment =TextAlignment.Start};
                    //sv.Content = lbl;
                    //  lbl.SetBinding(Label.TextProperty, key.Name.ToString());
                    grid.Children.Add(lbl, colCounter, grid.RowDefinitions.Count - 1);
                    colCounter++;
                }
                if ((PassbookView2.rowCounter % 2) == 0)
                { grid.BackgroundColor = Xamarin.Forms.Color.FromHex("#b7dae5"); }

                else
                { grid.BackgroundColor = Xamarin.Forms.Color.FromHex("#fcfcfc"); }

                PassbookView2.rowCounter++;
                // }
                View = grid;



            }
        }
    }
}
