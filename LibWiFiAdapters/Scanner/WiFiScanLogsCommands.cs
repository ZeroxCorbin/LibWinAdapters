//using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace LibWiFiAdapters.Scanner
{
    public class WiFiScanEntryCommands
    {
        private string _DatabaseFilePath;
        private string _DatabaseFileName;
        public string DatabaseFileFullName
        {
            get
            {
                return _DatabaseFilePath + "\\" + _DatabaseFileName;
            }
        }

        private SQLiteConnection _DbConnection = null;

        public WiFiScanEntryCommands(string fileFullName, bool overwrite)
        {
            if (string.IsNullOrEmpty(fileFullName)) return;

            _DatabaseFileName = Path.GetFileName(fileFullName);
            _DatabaseFilePath = Path.GetDirectoryName(fileFullName);

            CreateDatabaseFile(overwrite);
            CreateWiFiScanEntriesTable();
        }

        private void CreateDatabaseFile(bool overwrite)
        {
            if (overwrite && File.Exists(DatabaseFileFullName)) File.Delete(DatabaseFileFullName);
            SQLiteConnection.CreateFile(DatabaseFileFullName);
        }

        private bool OpenConnection()
        {
            if (_DbConnection == null) _DbConnection = new SQLiteConnection("Data Source=" + DatabaseFileFullName + "; Version=3;");

            if (_DbConnection.State == System.Data.ConnectionState.Closed) _DbConnection.Open();

            if (_DbConnection.State != System.Data.ConnectionState.Open) return false;
            else return true;
        }

        private void CreateWiFiScanEntriesTable()
        {
            if (!OpenConnection()) return;
            string sqlTable = "CREATE TABLE WiFiScanEntries (entry_id integer PRIMARY KEY," +
                "Timestamp TEXT NOT NULL," +
                "Ssid TEXT NULL," +
                "Bssid TEXT NOT NULL," +
                "ChannelCenterFrequencyInKilohertz INTEGER NOT NULL," +
                "NetworkRssiInDecibelMilliwatts REAL NOT NULL," +
                "SignalBars INTEGER NOT NULL," +
                "Uptime TEXT NOT NULL," +
                "Location TEXT NOT NULL);";

            using (SQLiteCommand command = new SQLiteCommand(sqlTable, _DbConnection))
            {
                command.ExecuteNonQuery();
            }

        }

        public void AddRow(Windows.Devices.WiFi.WiFiAvailableNetwork avn, GeoJSON.Net.Feature.Feature cords)
        {
            if (!OpenConnection()) return;
            string sqlInsert = "insert into WiFiScanEntries (Timestamp, Ssid, Bssid, ChannelCenterFrequencyInKilohertz, NetworkRssiInDecibelMilliwatts, SignalBars, Uptime, Location)" +
                "values ('" + DateTime.Now +
                "', '" + avn.Ssid +
                "', '" + avn.Bssid +
                "', " + avn.ChannelCenterFrequencyInKilohertz +
                ", " + avn.NetworkRssiInDecibelMilliwatts +
                ", " + avn.SignalBars +
                ", '" + avn.Uptime +
                "', '" + Newtonsoft.Json.JsonConvert.SerializeObject(cords, Newtonsoft.Json.Formatting.None) + "')";

            using (SQLiteCommand command = new SQLiteCommand(sqlInsert, _DbConnection))
            {
                command.ExecuteNonQuery();
            }
        }

        public void WriteToXML(string file)
        {

        }
    }
}
