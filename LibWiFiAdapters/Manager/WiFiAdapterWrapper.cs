using System;
using System.Threading.Tasks;
using Windows.Devices.WiFi;

namespace LibWiFiAdapters.Manager
{
    public class WiFiAdapterWrapper : IDisposable
    {
        //public event ScanComplete_EventHandler ScanComplete;
        public delegate void ScanComplete_EventHandler(WiFiAdapter adapter);
        public delegate void Connected_EventHandler(WiFiConnectionResult results);

        private event ScanComplete_EventHandler _ScanComplete;
        public event ScanComplete_EventHandler ScanComplete
        {
            add { if (HandlingScanCompleteEvent) return; _ScanComplete += value; }
            remove { _ScanComplete -= value; }
        }
        public bool HandlingScanCompleteEvent { get { return _ScanComplete != null; } }

        private event Connected_EventHandler _Connected;
        public event Connected_EventHandler Connected
        {
            add { if (HandlingConnectedEvent) return; _Connected += value; }
            remove { _Connected -= value; }
        }
        public bool HandlingConnectedEvent { get { return _Connected != null; } }

        private WiFiAdapter _Adapter;
        public WiFiAdapter Adapter { get { return _Adapter; } }

        private string _DeviceID;
        public string DeviceID { get { return _DeviceID; } }

        private long _LastScanTime;
        public long LastScanTime { get { return _LastScanTime; } }

        private System.Diagnostics.Stopwatch _StopWatch;

        public WiFiAdapterWrapper(WiFiAdapter adp, string devId = "")
        {
            _LastScanTime = 0;
            _StopWatch = new System.Diagnostics.Stopwatch();

            _Adapter = adp;
            _Adapter.AvailableNetworksChanged += Adapter_AvailableNetworksChanged;
            _DeviceID = devId;
        }

        public void Dispose()
        {
            if (_ScanComplete != null) _ScanComplete = null;
            _Adapter.AvailableNetworksChanged -= Adapter_AvailableNetworksChanged;

            _ScanComplete = null;
            _Connected = null;

            _Adapter = null;
            _StopWatch = null;
            _DeviceID = null;
        }


        public Task<WiFiConnectionResult> Connect(WiFiAvailableNetwork availableNetwork, WiFiReconnectionKind reconnectionKind)
        {
            return Task.Run(() => __Connect(availableNetwork, reconnectionKind));
        }

        private async Task<WiFiConnectionResult> __Connect(WiFiAvailableNetwork availableNetwork, WiFiReconnectionKind reconnectionKind)
        {
            if (_Adapter != null)
            {
                WiFiConnectionResult tsk = await _Adapter.ConnectAsync(availableNetwork, reconnectionKind);
                _Connected?.Invoke(tsk);
                return tsk;
            }
            return null;
        }


        public Task ScanForNetworks()
        {
            _StopWatch.Restart();
            return Task.Run(() => __ScanForNetworks());
        }

        private async Task __ScanForNetworks()
        {
            if (_Adapter != null)
            {
                await _Adapter.ScanAsync();
            }
        }

        private void Adapter_AvailableNetworksChanged(WiFiAdapter sender, object args)
        {
            if (_StopWatch == null) return;
            _StopWatch.Stop();
            _LastScanTime = _StopWatch.ElapsedMilliseconds;
            _ScanComplete?.Invoke(sender);
        }

    }
}
