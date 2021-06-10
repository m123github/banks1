using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace App2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
       // SqlHelper.DeleteTable<User>();
            SqlHelper.AddTable<Passbook1>();
           
            SqlHelper.AddTable<User>();
            var currentUser = SqlHelper.GetConnection().Table<User>().FirstOrDefault();
            if (currentUser != null)
                MainPage = new NavigationPage(new Login(currentUser.BankId));
            else
               
           MainPage = new NavigationPage(new Registration());

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
