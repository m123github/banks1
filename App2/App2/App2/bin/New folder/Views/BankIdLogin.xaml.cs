using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BankIdLogin : ContentPage
	{

        private SQLiteConnection conn;
        public BankIdLogin ()
		{
            NavigationPage.SetHasNavigationBar(this, false);
            conn = SqlHelper.GetConnection();
            
            InitializeComponent ();

            LoginValues();
            
            // BankId.Text = "214";
		}
        private async void NavigateButton_OnClicked_proceed(object sender, EventArgs e)
        {

           


            if (string.IsNullOrEmpty(BankId.Text))
            {

                await DisplayAlert("Alert", "Please enter Bank ID", "Ok");
                return;

            }

            Application.Current.Properties["BankId"] = BankId.Text;

            await Navigation.PushAsync(new Login(BankId.Text)); 

            
        }

        public void LoginValues()
        {
            var a= conn.Table<User>().FirstOrDefault();

            if (a != null)
            {


                BankId.Text = conn.Table<User>().SingleOrDefault().BankId;

            }



        }


    }
}