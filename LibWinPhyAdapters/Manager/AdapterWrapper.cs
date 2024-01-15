using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LibWinPhyAdapters.Manager
{
    public class AdapterWrapper : IDisposable
    {
        private ManagementObject _man;
        public AdapterWrapper(ManagementObject m)
        {
            _man = m;
        }

        private object GetParameter(string paramName, Type type)
        {
            object obj = _man.GetPropertyValue(paramName);
            if (obj != null) return obj;
            if (type.IsValueType) return Activator.CreateInstance(type);
            if (type.IsArray) return Activator.CreateInstance(type, new object[] { 1 });
            return null;
        }
        private uint InvokeMethod(string methodName, ManagementBaseObject man, InvokeMethodOptions options)
        {
            ManagementBaseObject obj = _man.InvokeMethod(methodName, man, options);
            object obj1 = obj["ReturnValue"];
            return (uint)obj1;
        }

        public object this[string propertyName]
        {
            get
            {
                PropertyInfo property = GetType().GetProperty(propertyName);
                return property.GetValue(this, null);
            }
            set
            {
                PropertyInfo property = GetType().GetProperty(propertyName);
                Type propType = property.PropertyType;
                if (value == null)
                {
                    if (propType.IsValueType && Nullable.GetUnderlyingType(propType) == null)
                    {
                        throw new InvalidCastException();
                    }
                    else
                    {
                        property.SetValue(this, null, null);
                    }
                }
                else if (value.GetType() == propType)
                {
                    property.SetValue(this, value, null);
                }
                else
                {
                    TypeConverter typeConverter = TypeDescriptor.GetConverter(propType);
                    object propValue = typeConverter.ConvertFromString(value.ToString());
                    property.SetValue(this, propValue, null);
                }
            }
        }

        readonly IDictionary<ushort, string> Availability_Dict = new Dictionary<ushort, string>
        {
            {0,"Undefined"},
{1,"Other"},
{2,"Unknown"},
{3,"Running/Full Power"},
{4,"Warning"},
{5,"In Test"},
{6,"Not Applicable"},
{7,"Power Off"},
{8,"Off Line"},
{9,"Off Duty"},
{10,"Degraded"},
{11,"Not Installed"},
{12,"Install Error"},
{13,"Power Save - Unknown"},
{14,"Power Save - Low Power Mode"},
{15,"Power Save - Standby"},
{16,"Power Cycle"},
{17,"Power Save - Warning"},
{18,"Paused"},
{19,"Not Ready"},
{20,"Not Configured"},
{21,"Quiesced"}

        };
        readonly IDictionary<ushort, string> NetConnectionStatus_Dict = new Dictionary<ushort, string>
        {
            {0,"Disconnected"},
            {1,"Connecting"},
            {2,"Connected"},
            {3,"Disconnecting"},
            {4,"Hardware Not Present"},
            {5,"Hardware Disabled"},
            {6,"Hardware Malfunction"},
            {7,"Media Disconnected"},
            {8,"Authenticating"},
            {9,"Authentication Succeeded"},
            {10,"Authentication Failed"},
            {11,"Invalid Address"},
            {12,"Credentials Required"},
        };
        readonly IDictionary<ushort, string> PowerManagementCapabilities_Dict = new Dictionary<ushort, string>
        {
            {0,"Unknown"},
            {1,"Not Supported"},
            {2,"Disabled"},
            {3,"Enabled"},
            {4,"Power Saving Modes Entered Automatically"},
            {5,"Power State Settable"},
            {6,"Power Cycling Supported"},
            {7,"Timed Power On Supported"}
        };
        readonly IDictionary<ushort, string> StatusInfo_Dict = new Dictionary<ushort, string>
        {
            {0,"Undefined"},
            {1,"Other"},
            {2,"Unknown"},
            {3,"Enabled"},
            {4,"Disabled"},
            {5,"Not Applicable"}
        };

        public void Dispose()
        {
            _man.Dispose();
        }

        public string AdapterType
        {
            get { return (string)GetParameter("AdapterType", typeof(string)); }
        }
        public ushort AdapterTypeID
        {
            get { return (ushort)GetParameter("AdapterTypeID", typeof(ushort)); }
        }
        public bool AutoSense
        {
            get { return (bool)GetParameter("AutoSense", typeof(bool)); }
        }
        public ushort Availability
        {
            get { return (ushort)GetParameter("Availability", typeof(ushort)); }
        }
        public string Availability_String
        {
            get { return Availability_Dict[Availability]; }
        }
        public string Caption
        {
            get { return (string)GetParameter("Caption", typeof(string)); }
        }
        public uint ConfigManagerErrorCode
        {
            get { return (uint)GetParameter("ConfigManagerErrorCode", typeof(uint)); }
        }
        public bool ConfigManagerUserConfig
        {
            get { return (bool)GetParameter("ConfigManagerUserConfig", typeof(bool)); }
        }
        public string CreationClassName
        {
            get { return (string)GetParameter("CreationClassName", typeof(string)); }
        }
        public string Description
        {
            get { return (string)GetParameter("Description", typeof(string)); }
        }
        public string DeviceID
        {
            get { return (string)GetParameter("DeviceID", typeof(string)); }
        }
        public bool ErrorCleared
        {
            get { return (bool)GetParameter("ErrorCleared", typeof(bool)); }
        }
        public string ErrorDescription
        {
            get { return (string)GetParameter("ErrorDescription", typeof(string)); }
        }
        public string GUID
        {
            get { return (string)GetParameter("GUID", typeof(string)); }
        }
        public uint Index
        {
            get { return (uint)GetParameter("Index", typeof(uint)); }
        }
        public DateTime InstallDate
        {
            get
            {
                object obj = GetParameter("InstallDate", typeof(DateTime));
                if (obj.GetType() == typeof(DateTime)) return (DateTime)obj;
                //DMTF datetime
                if (obj.GetType() == typeof(string)) return ManagementDateTimeConverter.ToDateTime((string)obj);
                else return new DateTime();
            }
        }
        public bool Installed
        {
            get { return (bool)GetParameter("Installed", typeof(bool)); }
        }
        public uint InterfaceIndex
        {
            get { return (uint)GetParameter("InterfaceIndex", typeof(uint)); }
        }
        public uint LastErrorCode
        {
            get { return (uint)GetParameter("LastErrorCode", typeof(uint)); }
        }
        public string MACAddress
        {
            get { return (string)GetParameter("MACAddress", typeof(string)); }
        }
        public string Manufacturer
        {
            get { return (string)GetParameter("Manufacturer", typeof(string)); }
        }
        public uint MaxNumberControlled
        {
            get { return (uint)GetParameter("MaxNumberControlled", typeof(uint)); }
        }
        public ulong MaxSpeed
        {
            get { return (ulong)GetParameter("MaxSpeed", typeof(ulong)); }
        }
        public string Name
        {
            get { return (string)GetParameter("Name", typeof(string)); }
        }
        public string NetConnectionID
        {
            get { return (string)GetParameter("NetConnectionID", typeof(string)); }
        }
        public ushort NetConnectionStatus
        {
            get { return (ushort)GetParameter("NetConnectionStatus", typeof(ushort)); }
        }
        public string NetConnectionStatus_String
        {
            get { return NetConnectionStatus_Dict[NetConnectionStatus]; }
        }
        public bool NetEnabled
        {
            get { return (bool)GetParameter("NetEnabled", typeof(bool)); }
        }
        public string[] NetworkAddresses
        {
            get { return (string[])GetParameter("NetworkAddresses", typeof(string[])); }
        }
        public string PermanentAddress
        {
            get { return (string)GetParameter("PermanentAddress", typeof(string)); }
        }
        public bool PhysicalAdapter
        {
            get { return (bool)GetParameter("PhysicalAdapter", typeof(bool)); }
        }
        public string PNPDeviceID
        {
            get { return (string)GetParameter("PNPDeviceID", typeof(string)); }
        }
        public ushort[] PowerManagementCapabilities
        {
            get { return (ushort[])GetParameter("PowerManagementCapabilities", typeof(ushort[])); }
        }
        public string PowerManagementCapabilities_String
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (ushort val in PowerManagementCapabilities) sb.AppendLine(PowerManagementCapabilities_Dict[val]);
                return sb.ToString();
            }
        }
        public bool PowerManagementSupported
        {
            get { return (bool)GetParameter("PowerManagementSupported", typeof(bool)); }
        }
        public string ProductName
        {
            get { return (string)GetParameter("ProductName", typeof(string)); }
        }
        public string ServiceName
        {
            get { return (string)GetParameter("ServiceName", typeof(string)); }
        }
        public ulong Speed
        {
            get { return (ulong)GetParameter("Speed", typeof(ulong)); }
        }
        public string Status
        {
            get { return (string)GetParameter("Status", typeof(string)); }
        }
        public ushort StatusInfo
        {
            get { return (ushort)GetParameter("StatusInfo", typeof(ushort)); }
        }
        public string StatusInfo_String
        {
            get { return StatusInfo_Dict[StatusInfo]; }
        }
        public string SystemCreationClassName
        {
            get { return (string)GetParameter("SystemCreationClassName", typeof(string)); }
        }
        public string SystemName
        {
            get { return (string)GetParameter("SystemName", typeof(string)); }
        }
        public DateTime TimeOfLastReset
        {
            get
            {
                object obj = GetParameter("TimeOfLastReset", typeof(DateTime));
                if (obj.GetType() == typeof(DateTime)) return (DateTime)obj;
                //DMTF datetime
                if (obj.GetType() == typeof(string)) return ManagementDateTimeConverter.ToDateTime((string)obj);
                else return new DateTime();
            }
        }

        public uint Disable() //Disables the network adapter.
        {
            return InvokeMethod("Disable", null, null);
        }
        public uint Enable() //Enables the network adapter.
        {
            return InvokeMethod("Enable", null, null);
        }
    }
}
