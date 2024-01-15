using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Windows.Devices.Enumeration;
using Windows.Devices.WiFi;

namespace LibWiFiAdapters.Manager
{
    public class WiFiAdapterManager : IDisposable
    {
        private event WiFiAdapterWrapper.ScanComplete_EventHandler _ScanComplete;
        public event WiFiAdapterWrapper.ScanComplete_EventHandler ScanComplete
        {
            add { if (HandlingEvents) return; _ScanComplete += value; }
            remove { _ScanComplete -= value; }
        }
        public bool HandlingEvents { get { return _ScanComplete != null; } }

        public event WiFiAdapterWrapper.Connected_EventHandler Connected;

        public static Dictionary<int, int> Channels = new Dictionary<int, int>()
        {
            {2412000,1},
            {2417000,2},
            {2422000,3},
            {2427000,4},
            {2432000,5},
            {2437000,6},
            {2442000,7},
            {2447000,8},
            {2452000,9},
            {2457000,10},
            {2462000,11},
            {2467000,12},
            {2472000,13},
            {2484000,14},
            {5180000,36},
            {5200000,40},
            {5220000,44},
            {5240000,48},
            {5260000,52},
            {5280000,56},
            {5300000,60},
            {5320000,64},
            {5500000,100},
            {5520000,104},
            {5540000,108},
            {5560000,112},
            {5580000,116},
            {5600000,120},
            {5620000,124},
            {5640000,128},
            {5660000,132},
            {5680000,136},
            {5700000,140},
            {5745000,149},
            {5765000,153},
            {5785000,157},
            {5805000,161},
            {5825000,165},
        };

        public static Dictionary<string, string> PHYKindDescriptions = new Dictionary<string, string>()
        {
            {"Dmg","Directional multi-gigabit (DMG) PHY for 802.11ad."},
            {"Dsss","Direct sequence, spread-spectrum (DSSS) PHY."},
            {"Erp","Extended Rate (ERP) PHY."},
            {"Fhss","Frequency-hopping, spread-spectrum (FHSS) PHY."},
            {"Hrdsss","High-rated DSSS (HRDSSS) PHY."},
            {"HT","High Throughput (HT) PHY for 802.11n PHY."},
            {"IRBaseband","Infrared (IR) baseband PHY."},
            {"Ofdm","Orthogonal frequency division multiplex (OFDM) PHY."},
            {"Unknown","Unspecified PHY type"},
            {"Vht","Very High Throughput (VHT) PHY for 802.11ac PHY."}
        };
        public static Dictionary<string, string> NetworkKindDescriptions = new Dictionary<string, string>()
        {
            {"Adhoc","An independent (IBSS) network."},
            {"Any","Either an infrastructure or independent network."},
            {"Infrastructure","An infrastructure network."}
        };
        public static Dictionary<string, string> NetworkAuthenticationTypeDescriptions = new Dictionary<string, string>()
        {
            {"Ihv","Specifies an authentication type defined by an independent hardware vendor (IHV)."},
            {"None","No authentication enabled."},
            {"Open80211","Open authentication over 802.11 wireless."},
            {"Rsna","Specifies an IEEE 802.11i Robust Security Network Association (RSNA) algorithm."},
            {"RsnaPsk","Specifies an IEEE 802.11i RSNA algorithm that uses PSK."},
            {"SharedKey80211","Specifies an IEEE 802.11 Shared Key authentication algorithm that requires the use of a pre-shared Wired Equivalent Privacy (WEP) key for the 802.11 authentication."},
            {"Unknown","Authentication method unknown."},
            {"Wpa","Specifies a Wi-Fi Protected Access (WPA) algorithm."},
            {"WpaNone","Wi-Fi Protected Access."},
            {"WpaPsk","Specifies a Wi-Fi Protected Access (WPA) algorithm that uses pre-shared keys (PSK)."}
        };
        public static Dictionary<string, string> NetworkEncryptionTypeDescriptions = new Dictionary<string, string>()
        {
            {"Ccmp","Specifies an AES-CCMP algorithm."},
            {"Ihv","Specifies an encryption type defined by an independent hardware vendor (IHV)."},
            {"None","No encryption enabled."},
            {"RsnUseGroup","Specifies a Robust Security Network (RSN) Use Group Key cipher suite."},
            {"Tkip","Specifies a Temporal Key Integrity Protocol (TKIP) algorithm."},
            {"Unknown","Encryption method unknown."},
            {"Wep","Specifies a WEP cipher algorithm with a cipher key of any length."},
            {"Wep104","Specifies a WEP cipher algorithm with a 104-bit cipher key."},
            {"Wep40","Specifies a Wired Equivalent Privacy (WEP) algorithm."},
            {"WpaUseGroup","Specifies a Wifi Protected Access (WPA) Use Group Key cipher suite."}

        };

        private List<WiFiAdapterWrapper> _Adapters = new List<WiFiAdapterWrapper>();
        public List<WiFiAdapterWrapper> Adapters { get { return _Adapters; } }

        public WiFiAdapterManager()
        {
            _Adapters = GetAdapterList();
        }

        public void Dispose()
        {
            ScanForNetworks_StopContinuous();

            foreach (WiFiAdapterWrapper adp in _Adapters)
            {
                adp.Dispose();
            }
            _Adapters.Clear();
            _Adapters = null;
        }

        public long LastScanTime
        {
            get
            {
                long sum = 0;
                long cnt = 0;
                foreach (WiFiAdapterWrapper adp in _Adapters)
                {
                    sum += adp.LastScanTime;
                    cnt++;
                }
                return sum / cnt;
            }
        }

        public List<WiFiAdapterWrapper> GetAdapterList()
        {
            try
            {
                using (Task<List<WiFiAdapterWrapper>> task = Task.Run(__GetAdapterList))
                {
                    task.Wait();
                    return task.Result;
                }
            }
            catch (AggregateException)
            {
                return new List<WiFiAdapterWrapper>();
            }
        }

        private async Task<List<WiFiAdapterWrapper>> __GetAdapterList()
        {
            List<WiFiAdapterWrapper> _Adapters = new List<WiFiAdapterWrapper>();

            var access = await WiFiAdapter.RequestAccessAsync();
            if (access != WiFiAccessStatus.Allowed)
            {
                throw new Exception("WiFiAccessStatus not allowed");
            }
            else
            {
                var wifiAdapterResults = await DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
                foreach (var dev in wifiAdapterResults)
                {
                    _Adapters.Add(new WiFiAdapterWrapper(await WiFiAdapter.FromIdAsync(dev.Id), dev.Id));
                }
            }
            return _Adapters;
        }


        public WiFiAdapterWrapper GetAdapter(string devID)
        {
            return _Adapters.Where(a => a.DeviceID.Equals(devID)).FirstOrDefault();
        }

        public WiFiAdapterWrapper GetAdapterBySettingID(string settingID)
        {
            string test = settingID.Trim(new char[] { '{', '}' });
            return _Adapters.Where(a => a.Adapter.NetworkAdapter.NetworkAdapterId.ToString().ToUpper().Equals(test)).FirstOrDefault();
        }

        private async Task<WiFiAdapterWrapper> __GetAdapter(string devID)
        {
            List<WiFiAdapterWrapper> _Adapters = new List<WiFiAdapterWrapper>();

            var access = await WiFiAdapter.RequestAccessAsync();
            if (access != WiFiAccessStatus.Allowed)
            {
                throw new Exception("WiFiAccessStatus not allowed");
            }
            else
            {
                return new WiFiAdapterWrapper(await WiFiAdapter.FromIdAsync(devID), devID);
            }
        }

        public void ScanForNetworks_Async(string devID)
        {
            using (WiFiAdapterWrapper adapter = GetAdapter(devID))
            {
                adapter.ScanForNetworks();
                adapter.ScanComplete += _ScanComplete_;
            }
        }

        public void ScanForNetworks_StartContinuous(string devID)
        {
            if (_Adapters.Count == 0) _Adapters = GetAdapterList();

            WiFiAdapterWrapper war = _Adapters.Where(m => m.DeviceID.Equals(devID)).FirstOrDefault();
            if (war == null) return;

            war.ScanComplete += _ScanComplete_;
            war.ScanForNetworks();
        }

        public void ScanForNetworks_StartContinuous()
        {
            foreach (WiFiAdapterWrapper csa in _Adapters)
            {
                if (csa.HandlingScanCompleteEvent) continue;
                csa.ScanComplete += _ScanComplete_;
                csa.ScanForNetworks();
            }
        }

        public void ScanForNetworks_StopContinuous()
        {
            foreach (WiFiAdapterWrapper csa in _Adapters) csa.ScanComplete -= _ScanComplete_;
        }

        public void ScanForNetworks_StopContinuous(string devID)
        {
            WiFiAdapterWrapper war = _Adapters.Where(m => m.DeviceID.Equals(devID)).FirstOrDefault();
            if (war == null) return;
            war.ScanComplete -= _ScanComplete_;

            bool active = false;
            foreach (WiFiAdapterWrapper csa in _Adapters) if (csa.HandlingScanCompleteEvent) active = true;
            if (active) return;

            foreach (WiFiAdapterWrapper csa in _Adapters)
                csa.Dispose();
            _Adapters.Clear();
        }

        private void _ScanComplete_(WiFiAdapter adapter)
        {
            _ScanComplete?.Invoke(adapter);

            foreach (WiFiAdapterWrapper csa in _Adapters)
            {
                csa.ScanForNetworks();
            }
        }

        public void Connect_Async(string devID, WiFiAvailableNetwork availableNetwork, WiFiReconnectionKind reconnectionKind)
        {
            using (WiFiAdapterWrapper adapter = GetAdapter(devID))
            {
                adapter.Connect(availableNetwork, reconnectionKind);
                adapter.Connected += Connected;
            }
        }


    }
}
