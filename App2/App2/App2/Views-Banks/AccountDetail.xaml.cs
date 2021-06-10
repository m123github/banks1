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
           listView1.SeparatorColor = Color.Blue;

            //lbl.Text = DisplayAccount.ADDITIONAL_INFO.Replace("=", new String(' ', 20)).Replace( "|", System.Environment.NewLine);
            string[] ab = DisplayAccount.ADDITIONAL_INFO.Split('|');
          //  List<string> l1 = new List<string>();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();




            foreach (var item in ab)
            {
                // int index =item.IndexOf('=');
                //var  key = item.Substring(0, index);
                // var value = item.Substring(item.LastIndexOf('=') + 1);

                string[] keyValue = item.Split('=');
                dictionary.Add(keyValue[0], keyValue[1]);

            }
            var a = dictionary;
            listView1.ItemsSource = dictionary;
        }
	}
}