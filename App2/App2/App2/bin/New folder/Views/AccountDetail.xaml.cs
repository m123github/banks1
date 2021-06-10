using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountDetail : PopupPage
	{
        public Account DisplayAccount { get; set; }
        public AccountDetail (Account Act)
		{
            DisplayAccount = Act;

            InitializeComponent ();
            Title = "Details";
            
            lbl.Text = DisplayAccount.ADDITIONAL_INFO.Replace( "|", System.Environment.NewLine).Trim();
            
		}
	}
}