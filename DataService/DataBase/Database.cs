using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace DataService.DataBase
{
    public static class Database
    {
        //Check if file exists and make a connection to DB
        private static SQLiteConnection GetConnection()
        {
            if (!File.Exists("SettingRecord.sqlite"))
            {
                SQLiteConnection.CreateFile("SettingRecord.sqlite");
            }
            SQLiteConnection dbConnection = new SQLiteConnection("Data Source=SettingRecord.sqlite;Version=3;");
            return dbConnection;
        }

        //Check if table exists in DB
        private static string CheckTableExist(SQLiteConnection connection)
        {
            string checkTable = "SELECT name, COUNT(*) FROM sqlite_master WHERE type='table' AND name='SettingsRecord'";
            SQLiteCommand command = new SQLiteCommand(checkTable, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            string count = string.Empty;
            while (reader.Read())
            {
                count = reader["name"].ToString();
            }
            return count;
        }

        //Inserting last result of searching to DB
        public static void InsertLastResult(string searchField, string searchResult)
        {
            SQLiteConnection dbConnection = GetConnection();
            dbConnection.Open();
            string checkTable = "SELECT name, COUNT(*) FROM sqlite_master WHERE type='table' AND name='SettingsRecord'";
            SQLiteCommand command = new SQLiteCommand(checkTable, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            string count = string.Empty;
            while (reader.Read())
            {
                count = reader["name"].ToString();
            }
            if (string.IsNullOrEmpty(CheckTableExist(dbConnection)))
            {
                string createTable = "create table SettingsRecord (ID integer primary key, SearchField text, SearchResult text, DateCreated date)";
                command = new SQLiteCommand(createTable, dbConnection);
                command.ExecuteNonQuery();
            }
            StringBuilder insert = new StringBuilder();
            insert.AppendFormat("insert into SettingsRecord(SearchField, SearchResult, DateCreated) values('{0}','{1}', datetime('now'))", searchField, searchResult);
            command = new SQLiteCommand(insert.ToString(), dbConnection);
            command.ExecuteNonQuery();
            dbConnection.Clone();
        }

        //Retrieving data from DB
        public static List<Setting> GetLastData()
        {
            List<Setting> result = new List<Setting>();
            SQLiteConnection dbConnection = GetConnection();
            dbConnection.Open();
            if (!string.IsNullOrEmpty(CheckTableExist(dbConnection)))
            {
                string getData = "select SearchField, SearchResult from SettingsRecord where DateCreated=(select max(DateCreated) from SettingsRecord)";
                SQLiteCommand command = new SQLiteCommand(getData, dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                string searchResult = string.Empty;
                while (reader.Read())
                {
                    searchResult = reader["SearchResult"].ToString();
                }
                dbConnection.Close();
                var json = new JavaScriptSerializer();
                result = json.Deserialize<List<Setting>>(searchResult);
            }
            return result;
        }

    }
}