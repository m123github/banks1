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
	public partial class AboutUs : ContentPage
	{
        public AboutUs()
        {
            InitializeComponent();
            Title = "AboutUs";
        }

        private void Btncontact_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EmailUs());
        }
        async private void MenuItem1_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Dashboard());
           // await Navigation.PopAsync();
        }
    }
}