using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App2.Droid;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(Sqlite_Android))]
namespace App2.Droid
{
    public class Sqlite_Android : ISqlliteInterface
    {
        public SQLiteConnection GetConnection()
        {
            string filename = "database1.db";
            string documentpath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string path = Path.Combine( documentpath,filename);
            SQLiteConnection Database = new SQLiteConnection(path);
           // Database.CreateTable<Employee>();
            return Database;
        }
    }
}