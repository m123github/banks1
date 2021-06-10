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
    public partial class MasterPage : ContentPage
    {
        public MasterPage()
        {
            InitializeComponent();
            List<MasterPageItem> menuItems = new List<MasterPageItem>
            {
                new MasterPageItem { Title="About Us", IconSource="App2.images.share.png",TargetType=typeof(PassbookView2)},
                new MasterPageItem { Title="Logout", IconSource="App2.images.fixedpst.png", TargetType=typeof(BankIdLogin) }
        };
            listView.ItemsSource = menuItems;

        }

        private void listView_ItemTapped(object sender, SelectedItemChangedEventArgs e)
        {

            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                if (item.Title == "Logout")
                { Application.Current.MainPage = new NavigationPage(new BankIdLogin()); }
                else
                   { new MainPage().Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType)); }


            }

        }
    }



        // var Detail = new NavigationPage((Page) Activator.CreateInstance(item.TargetType));



        public class ColorConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {

                if (targetType == typeof(ImageSource))
                {
                    var imageSource = ImageSource.FromResource(value.ToString(), typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                    return imageSource;
                }
                if (value.ToString() == "Cr")
                    return Color.Green;

                return Color.Maroon;

            }



            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }


        }
    }

