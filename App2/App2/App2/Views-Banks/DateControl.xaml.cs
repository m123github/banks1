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
    public partial class DateControl : ContentView
    {
        public static readonly BindableProperty DateProperty = BindableProperty.Create("Date", typeof(string), typeof(DateControl), default(string), Xamarin.Forms.BindingMode.OneWay);
        public string Date
        {
            get
            {
                return (string)GetValue(DateProperty);
            }

            set
            {
                SetValue(DateProperty, value);
            }
        }

        public static readonly BindableProperty MonthandYearProperty = BindableProperty.Create("MonthandYear", typeof(string), typeof(DateControl), default(string), BindingMode.TwoWay);
        public string MonthandYear
        {
            get
            {
                return (string)GetValue(MonthandYearProperty);
            }

            set
            {
                SetValue(MonthandYearProperty, value);
            }
        }
        public DateControl()
        {
            InitializeComponent();
            lblDate.Text = Date;
            lblMonthYear.Text = MonthandYear;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == DateProperty.PropertyName)
            {
                DateTime dt = DateTime.ParseExact(Date, "dd/MM/yyyy", null);
                lblDate.Text = dt.Day.ToString();
            }
            else if (propertyName == MonthandYearProperty.PropertyName)
            {
                DateTime dt = DateTime.ParseExact(MonthandYear, "dd/MM/yyyy", null);
                lblMonthYear.Text = dt.ToString("MMM") + " '" + dt.ToString("yy");
            }
        }
    }
}