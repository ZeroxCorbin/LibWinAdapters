using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibWiFiAdapters.Manager;
using Windows.Devices.WiFi;

namespace LibWiFiAdapters.Scanner
{
    public class WiFiBackgroundScan
    {
        public WiFiAdapterManager Adapter = new WiFiAdapterManager();

        private bool _IsScanning;
        public bool IsScanning { get { return _IsScanning; } }

        private bool loggingEnabled;
        public bool LoggingEnabled { get { return loggingEnabled; } }

        public WiFiBackgroundScan()
        {
            //Program.Position.PositionUpdated += _Position_Updated;
        }

        public void ScanStart(string adapterID)
        {
            _IsScanning = true;
            Adapter.ScanComplete += _ScanComplete;
            Adapter.ScanForNetworks_StartContinuous(adapterID);
        }

        public void ScanStart()
        {
            _IsScanning = true;
            Adapter.ScanComplete += _ScanComplete;
            Adapter.ScanForNetworks_StartContinuous();
        }

        public void ScanStop()
        {
            if (ScanComplete == null)
            {
                Adapter.ScanComplete -= _ScanComplete;
                Adapter.ScanForNetworks_StopContinuous();

                _IsScanning = false;
            }
        }

        public event EventHandler Position_Updated;
        private void _Position_Updated(object sender, EventArgs e)
        {
            Position_Updated?.Invoke(sender, e);
        }

        public event WiFiAdapterWrapper.ScanComplete_EventHandler ScanComplete;

        bool alt = false;
        private void _ScanComplete(WiFiAdapter adapter)
        {
            try
            {
                //if (Program.Settings.WiFiLogs.BackgroundLoggingEnabled || Program.Settings.WiFiLogs.LoggingEnabled)
                //{
                //    foreach (WiFiAvailableNetwork avn in adapter.NetworkReport.AvailableNetworks)
                //        Program.ScanEntries.AddRow(avn, Program.Position.GeoJsonFeature);

                //    if (alt)
                //    {
                //        if (Program.IsDarkTheme) Program.TrayIcon.Icon = Program.ChangeImageColor(Properties.Resources.netOKLogging_4Dark, Program.Settings.Colors.White, System.Drawing.Color.Green);
                //        else Program.TrayIcon.Icon = Program.ChangeImageColor(Properties.Resources.netOK_4Light, Program.Settings.Colors.Black, System.Drawing.Color.Green);
                //        alt = false;
                //    }
                //    else
                //    {
                //        if (Program.IsDarkTheme) Program.TrayIcon.Icon = Program.ChangeImageColor(Properties.Resources.netOKLogging_4Dark, Program.Settings.Colors.White, Program.Settings.Colors.TrayIconColor);
                //        else Program.TrayIcon.Icon = Program.ChangeImageColor(Properties.Resources.netOK_4Light, Program.Settings.Colors.Black, Program.Settings.Colors.TrayIconColor);
                //        alt = true;
                //    }
                //}
                //else
                //{
                //    if (alt)
                //    {
                //        if (Program.IsDarkTheme) Program.TrayIcon.Icon = Program.ChangeImageColor(Properties.Resources.netOK_4Dark, Program.Settings.Colors.White, System.Drawing.Color.Green);
                //        else Program.TrayIcon.Icon = Program.ChangeImageColor(Properties.Resources.netOK_4Light, Program.Settings.Colors.Black, System.Drawing.Color.Green);
                //        alt = false;
                //    }
                //    else
                //    {
                //        if (Program.IsDarkTheme) Program.TrayIcon.Icon = Program.ChangeImageColor(Properties.Resources.netOK_4Dark, Program.Settings.Colors.White, Program.Settings.Colors.TrayIconColor);
                //        else Program.TrayIcon.Icon = Program.ChangeImageColor(Properties.Resources.netOK_4Light, Program.Settings.Colors.Black, Program.Settings.Colors.TrayIconColor);
                //        alt = true;
                //    }
                //}



                //if (ScanComplete == null && !Program.Settings.WiFiLogs.BackgroundLoggingEnabled)
                //    ScanStop();

                ScanComplete?.Invoke(adapter);
            }
            catch (ObjectDisposedException)
            {

            }
        }
    }
}
