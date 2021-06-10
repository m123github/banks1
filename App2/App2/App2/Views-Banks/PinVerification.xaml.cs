using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;

namespace App2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PinVerification : PopupPage
    {

        private string otp;
        public PinVerification(string OTP)
        {


            InitializeComponent();

            otp = OTP;
            this.Title = "OTP Verification";

            var a = 1;


        }

        protected override void OnAppearing()
        {

         //  DisplayAlert("Alert", "OTP is " + otp + "", "Ok");

            base.OnAppearing();

        }

        async private void Button_Clicked(object sender, EventArgs e)
        {


            try
            {
                UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
               
                await Task.Delay(1000);

                if (!string.IsNullOrEmpty(entry1.Text))
            {

                if (otp == entry1.Text)
                {
                  //  await DisplayAlert("Success", "You have entered correct OTP", "Proceed");

                    otpcheck otpcheck = new otpcheck { value1 ="Ok"};


                    await Navigation.PopPopupAsync();
                    MessagingCenter.Send<otpcheck>(otpcheck, "Receiveddata");

                   

                }
                else
                {
                    await DisplayAlert("Sorry", "Wrong OTP, Please try again", "Ok");

                   // otpcheck otpcheck = new otpcheck { value1 = "Fail" };
                    await Navigation.PopPopupAsync();
                   // MessagingCenter.Send<otpcheck>(otpcheck, "Receiveddata");

                }

               

            }

            else
            {
                await DisplayAlert("Alert", "Please enter OTP", "Proceed");



            }
            }
            finally
            {

                UserDialogs.Instance.HideLoading();
               
            }
        }

      async  private void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();

        }
    }
}