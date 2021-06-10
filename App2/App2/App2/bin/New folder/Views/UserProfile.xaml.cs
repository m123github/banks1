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
	public partial class UserProfile : ContentPage
	{
        public bool IsEditing;
        public UserProfile ()
		{
			InitializeComponent ();
            Title = "My Profile";
            
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
         //   IsEditing = true;
          
        }
    }
}