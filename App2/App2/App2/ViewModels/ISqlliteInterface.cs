using PCLStorage;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;


namespace App2
{
    public interface ISqlliteInterface
    {
        SQLiteConnection GetConnection();
    }

    public class SqlHelper
    {

        public static object locker = new object();
        SQLiteConnection database;
        //public SqlHelper()
        //{
        //    database = GetConnection();
        //    // create the tables  
        //   // database.CreateTable<RegEntity>();
        //}
        public static SQLite.SQLiteConnection GetConnection()
        {
            SQLiteConnection sqlitConnection;
            var sqliteFilename = "Passbook.db3";
            IFolder folder = FileSystem.Current.LocalStorage;
            string path = PortablePath.Combine(folder.Path.ToString(), sqliteFilename);
            sqlitConnection = new SQLite.SQLiteConnection(path);

            return sqlitConnection;
        }

        public static bool TableExists<T>()
        {
            var connection = GetConnection();
            const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
            var cmd = connection.CreateCommand(cmdText, typeof(T).Name);
            return cmd.ExecuteScalar<string>() != null;
        }

        public static void AddTable<T>()
        {
            var connection = GetConnection();
            lock (locker)
                connection.CreateTable<T>();
        }

        public static void DeleteTable<T>()
        {
            var connection = GetConnection();
            lock (locker)
                connection.DropTable<T>();
        }
    }
}
