using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Dashboard : ContentPage
        
	{

        private Accountlist Accountlist;
        public string bankName { get; } = "";

        string BankId = "";
        string CustId = "";
        public  Customer info { get; }
        public static Customer info1 { get; set; }
        bool premiumvisible;
        //Customer cust
        public Dashboard():this(Dashboard.info1)
        {
          
        }
        public Dashboard(Customer cust)
        {


            ImageConverter convert = new ImageConverter();
          info = cust;
            Dashboard.info1 = cust;
            Title = "Home";
            
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);
           
            if (Application.Current.Properties.ContainsKey("BankId"))
            {
                BankId = Convert.ToString(Application.Current.Properties["BankId"]);


            }
            if (Application.Current.Properties.ContainsKey("CustId"))
            {
                CustId = Convert.ToString(Application.Current.Properties["CustId"]);
            }
            bankName = Application.Current.Properties["bankname"].ToString();
            BindingContext = this;
            Customer obj = new Customer();
            Accountlist = new Accountlist(CustId, BankId);
            var b = Accountlist.Versiondesc.ToLower();
           


            NavigationPage.SetHasBackButton(this, false);
         //   NavigationPage.SetTitleIcon(this, "Logout.png");
            if (b == "trial")
            {
                premiumpic.IsVisible = false;
                
                // Recent.IsVisible = true;
                trans.Source = (ImageSource)convert.Convert("App2.images.fundtransfer_disabled.png", typeof(ImageSource), null, null);
                myacc.Source = (ImageSource)convert.Convert("App2.images.myaccounts.png", typeof(ImageSource), null, null);
                fundtrans.Source = (ImageSource)convert.Convert("App2.images.fundtransfer.png", typeof(ImageSource), null, null);
                // pass.Source = (ImageSource)convert.Convert("App2.images.passbook_disabled.png", typeof(ImageSource), null, null);
                pass.Source = (ImageSource)convert.Convert("App2.images.passbook.png", typeof(ImageSource), null, null);
                bill.Source = (ImageSource)convert.Convert("App2.images.billpayement_disabled.png", typeof(ImageSource), null, null);
                atm.Source = (ImageSource)convert.Convert("App2.images.atm_disabled.png", typeof(ImageSource), null, null);
                trans.IsEnabled = false;
                // fundtrans.IsEnabled = false;
                //   pass.IsEnabled = false;
                bill.IsEnabled = false;
                atm.IsEnabled = false;


            }
            else
            {
                premiumpic.IsVisible = true;
                trans.Source = (ImageSource)convert.Convert("App2.images.recenttransaction.png", typeof(ImageSource), null, null);
                myacc.Source = (ImageSource)convert.Convert("App2.images.myaccounts.png", typeof(ImageSource), null, null);
                fundtrans.Source = (ImageSource)convert.Convert("App2.images.fundtransfer.png", typeof(ImageSource), null, null);
                pass.Source = (ImageSource)convert.Convert("App2.images.passbook.png", typeof(ImageSource), null, null);
                bill.Source = (ImageSource)convert.Convert("App2.images.billpayement.png", typeof(ImageSource), null, null);
                atm.Source = (ImageSource)convert.Convert("App2.images.atm.png", typeof(ImageSource), null, null);
            }
        }
        Image im = new Image();

        void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {



            if (((TappedEventArgs)args).Parameter.ToString() == "MyAccount")
            {
                //Navigation.InsertPageBefore(new BranchLocator(), this);
                //Navigation.PopAsync();
                Navigation.PushAsync(new AccountTypes1());
            }
            if (((TappedEventArgs)args).Parameter.ToString() == "RecentTransactions")
            {
                //Navigation.InsertPageBefore(new BranchLocator(), this);
                //Navigation.PopAsync();
                Navigation.PushAsync(new DetailedTransactions());
            }
            if (((TappedEventArgs)args).Parameter.ToString() == "Passbook")
            {
                //Navigation.InsertPageBefore(new BranchLocator(), this);
                //Navigation.PopAsync();
                Navigation.PushAsync(new PassbookView2());
            }



            if (((TappedEventArgs)args).Parameter.ToString() == "Fundtransfer")
            {
                //Navigation.InsertPageBefore(new BranchLocator(), this);
                //Navigation.PopAsync();
                Navigation.PushAsync(new FundTransfer(new FundTransferModel()));
            }

        }

      async  private void btnpinchange_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new pin(CustId, BankId));
        }

        private async void NavigateButton_AboutUS(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutUs());
        }

        async  private void btnabtus_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EmailUs());
        }

      async  private void MenuItem1_Clicked(object sender, EventArgs e)
        {

            try
            {
                UserDialogs.Instance.ShowLoading("Signing out", MaskType.Gradient);
                await Task.Delay(3000);

                if (Device.RuntimePlatform == Device.Android)
                    DependencyService.Get<IAndroidMethods>().CloseApp();
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }

        }
    }

    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null)
                return false;
            if (targetType == typeof(ImageSource))
            {
                var imageSource = ImageSource.FromResource(value.ToString(), typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                return imageSource;
            }
            else if (parameter != null)
            {
                return "";
            }



            return Color.Maroon;

        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
 
   
    

   
    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }

            // Do your translation lookup here, using whatever method you require
            var imageSource = ImageSource.FromResource(Source, typeof(ImageResourceExtension).GetTypeInfo().Assembly);

            return imageSource;
        }
    }


}