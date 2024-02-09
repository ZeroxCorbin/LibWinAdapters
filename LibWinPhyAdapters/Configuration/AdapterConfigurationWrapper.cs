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

namespace LibWinPhyAdapters.Configuration
{
    public class AdapterConfigurationWrapper : IDisposable
    {
        private ManagementObject _man;
        public AdapterConfigurationWrapper(ManagementObject m)
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
        private uint InvokeMethod(string methodName, ManagementBaseObject? man, InvokeMethodOptions? options)
        {
            ManagementBaseObject obj = _man.InvokeMethod(methodName, man, options);
            object obj1 = obj["ReturnValue"];
            return (uint)obj1;
        }

        public bool rebootRequired = false;

        public void Dispose()
        {
            _man.Dispose();
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

        public string GetCleanCaption()
        {
            return Caption.Remove(0, Caption.LastIndexOf("]") + 2);
        }

        public string Caption
        {
            get { return (string)GetParameter("Caption", typeof(string)); }
        }
        public string Description
        {
            get { return (string)GetParameter("Description", typeof(string)); }
        }
        public string SettingID
        {
            get { return (string)GetParameter("SettingID", typeof(string)); }
        }
        public bool ArpAlwaysSourceRoute
        {
            get { return (bool)GetParameter("ArpAlwaysSourceRoute", typeof(bool)); }
        }
        public bool ArpUseEtherSNAP
        {
            get { return (bool)GetParameter("ArpUseEtherSNAP", typeof(bool)); }
        }
        public string DatabasePath
        {
            get { return (string)GetParameter("DatabasePath", typeof(string)); }
        }
        public bool DeadGWDetectEnabled
        {
            get { return (bool)GetParameter("DeadGWDetectEnabled", typeof(bool)); }
        }
        public string[] DefaultIPGateway
        {
            get { return (string[])GetParameter("DefaultIPGateway", typeof(string[])); }
        }
        public uint DefaultTOS
        {
            get { return (uint)GetParameter("DefaultTOS", typeof(uint)); }
        }
        public uint DefaultTTL
        {
            get { return (uint)GetParameter("DefaultTTL", typeof(uint)); }
        }
        public bool DHCPEnabled
        {
            get { return (bool)GetParameter("DHCPEnabled", typeof(bool)); }

        }
        public DateTime DHCPLeaseExpires
        {
            get
            {
                object obj = GetParameter("DHCPLeaseExpires", typeof(DateTime));
                if (obj.GetType() == typeof(DateTime)) return (DateTime)obj;
                //DMTF datetime
                if (obj.GetType() == typeof(string)) return ManagementDateTimeConverter.ToDateTime((string)obj);
                else return new DateTime();
            }
        }
        public DateTime DHCPLeaseObtained
        {
            get
            {
                object obj = GetParameter("DHCPLeaseObtained", typeof(DateTime));
                if (obj.GetType() == typeof(DateTime)) return (DateTime)obj;
                //DMTF datetime
                if (obj.GetType() == typeof(string)) return ManagementDateTimeConverter.ToDateTime((string)obj);
                else return new DateTime();
            }
        }
        public string DHCPServer
        {
            get { return (string)GetParameter("DHCPServer", typeof(string)); }
        }
        public string DNSDomain
        {
            get { return (string)GetParameter("DNSDomain", typeof(string)); }
        }
        public string[] DNSDomainSuffixSearchOrder
        {
            get { return (string[])GetParameter("DNSDomainSuffixSearchOrder", typeof(string[])); }
        }
        public bool DNSEnabledForWINSResolution
        {
            get { return (bool)GetParameter("DNSEnabledForWINSResolution", typeof(bool)); }
        }
        public string DNSHostName
        {
            get { return (string)GetParameter("DNSHostName", typeof(string)); }
        }
        public string[] DNSServerSearchOrder
        {
            get { return (string[])GetParameter("DNSServerSearchOrder", typeof(string[])); }
        }
        public bool DomainDNSRegistrationEnabled
        {
            get { return (bool)GetParameter("DomainDNSRegistrationEnabled", typeof(bool)); }
        }
        public uint ForwardBufferMemory
        {
            get { return (uint)GetParameter("ForwardBufferMemory", typeof(uint)); }
        }
        public bool FullDNSRegistrationEnabled
        {
            get { return (bool)GetParameter("FullDNSRegistrationEnabled", typeof(bool)); }
        }
        public ushort[] GatewayCostMetric
        {
            get { return (ushort[])GetParameter("GatewayCostMetric", typeof(ushort[])); }
        }
        public uint IGMPLevel
        {
            get { return (uint)GetParameter("IGMPLevel", typeof(uint)); }
        }
        public uint Index
        {
            get { return (uint)GetParameter("Index", typeof(uint)); }
        }
        public uint InterfaceIndex
        {
            get { return (uint)GetParameter("InterfaceIndex", typeof(uint)); }
        }
        public string[] IPAddress
        {
            get { return (string[])GetParameter("IPAddress", typeof(string[])); }
        }
        public string IPAddressString
        {
            get
            {
                try
                {
                    string[] lst = (string[])_man.GetPropertyValue("IPAddress");
                    StringBuilder sb = new StringBuilder();
                    foreach (string s in lst)
                    {
                        sb.Append(s + " , ");
                    }
                    return sb.ToString();
                }
                catch { return string.Empty; }
            }
        }
        public uint IPConnectionMetric
        {
            get { return (uint)GetParameter("IPConnectionMetric", typeof(uint)); }
        }
        public bool IPEnabled
        {
            get { return (bool)GetParameter("IPEnabled", typeof(bool)); }
        }
        public bool IPFilterSecurityEnabled
        {
            get { return (bool)GetParameter("IPFilterSecurityEnabled", typeof(bool)); }
        }
        public bool IPPortSecurityEnabled
        {
            get { return (bool)GetParameter("IPPortSecurityEnabled", typeof(bool)); }
        }
        public string[] IPSecPermitIPProtocols
        {
            get { return (string[])GetParameter("IPSecPermitIPProtocols", typeof(string[])); }
        }
        public string[] IPSecPermitTCPPorts
        {
            get { return (string[])GetParameter("IPSecPermitTCPPorts", typeof(string[])); }
        }
        public string[] IPSecPermitUDPPorts
        {
            get { return (string[])GetParameter("IPSecPermitUDPPorts", typeof(string[])); }
        }
        public string[] IPSubnet
        {
            get { return (string[])GetParameter("IPSubnet", typeof(string[])); }
        }
        public bool IPUseZeroBroadcast
        {
            get { return (bool)GetParameter("IPUseZeroBroadcast", typeof(bool)); }
        }
        public string IPXAddress
        {
            get { return (string)GetParameter("IPXAddress", typeof(string)); }
        }
        public bool IPXEnabled
        {
            get { return (bool)GetParameter("IPXEnabled", typeof(bool)); }
        }
        public uint[] IPXFrameType
        {
            get { return (uint[])GetParameter("IPXFrameType", typeof(uint[])); }
        }
        public uint IPXMediaType
        {
            get { return (uint)GetParameter("IPXMediaType", typeof(uint)); }
        }
        public string[] IPXNetworkNumber
        {
            get { return (string[])GetParameter("IPXNetworkNumber", typeof(string[])); }
        }
        public string IPXVirtualNetNumber
        {
            get { return (string)GetParameter("IPXVirtualNetNumber", typeof(string)); }
        }
        public uint KeepAliveInterval
        {
            get { return (uint)GetParameter("KeepAliveInterval", typeof(uint)); }
        }
        public uint KeepAliveTime
        {
            get { return (uint)GetParameter("KeepAliveTime", typeof(uint)); }
        }
        public string MACAddress
        {
            get { return (string)GetParameter("MACAddress", typeof(string)); }
        }
        public uint MTU
        {
            get { return (uint)GetParameter("MTU", typeof(uint)); }
        }
        public uint NumForwardPackets
        {
            get { return (uint)GetParameter("NumForwardPackets", typeof(uint)); }
        }
        public bool PMTUBHDetectEnabled
        {
            get { return (bool)GetParameter("PMTUBHDetectEnabled", typeof(bool)); }
        }
        public bool PMTUDiscoveryEnabled
        {
            get { return (bool)GetParameter("PMTUDiscoveryEnabled", typeof(bool)); }
        }
        public string ServiceName
        {
            get { return (string)GetParameter("ServiceName", typeof(string)); }
        }
        public uint TcpipNetbiosOptions
        {
            get { return (uint)GetParameter("TcpipNetbiosOptions", typeof(uint)); }
        }
        public uint TcpMaxConnectRetransmissions
        {
            get { return (uint)GetParameter("TcpMaxConnectRetransmissions", typeof(uint)); }
        }
        public uint TcpMaxDataRetransmissions
        {
            get { return (uint)GetParameter("TcpMaxDataRetransmissions", typeof(uint)); }
        }
        public uint TcpNumConnections
        {
            get { return (uint)GetParameter("TcpNumConnections", typeof(uint)); }
        }
        public bool TcpUseRFC1122UrgentPointer
        {
            get { return (bool)GetParameter("TcpUseRFC1122UrgentPointer", typeof(bool)); }
        }
        public ushort TcpWindowSize
        {
            get { return (ushort)GetParameter("TcpWindowSize", typeof(ushort)); }
        }
        public bool WINSEnableLMHostsLookup
        {
            get { return (bool)GetParameter("WINSEnableLMHostsLookup", typeof(bool)); }
        }
        public string WINSHostLookupFile
        {
            get { return (string)GetParameter("WINSHostLookupFile", typeof(string)); }
        }
        public string WINSPrimaryServer
        {
            get { return (string)GetParameter("WINSPrimaryServer", typeof(string)); }
        }
        public string WINSScopeID
        {
            get { return (string)GetParameter("WINSScopeID", typeof(string)); }
        }
        public string WINSSecondaryServer
        {
            get { return (string)GetParameter("WINSSecondaryServer", typeof(string)); }
        }

        /// <summary> Disables IPsec on this TCP/IP-enabled network adapter.<summary/>
        public void DisableIPSec() { }
        /// <summary> Enables the Domain Name System (DNS) for service on this TCP/IP-bound network adapter.<summary/>
        public void EnableDNS() { }
        /// <summary> Enables IPsec on this specific TCP/IP-enabled network adapter.<summary/>
        public void EnableIPSec() { }
        /// <summary> Enables WINS settings specific to TCP/IP, but independent of the network adapter.<summary/>
        public void EnableWINS() { }
        /// <summary> Releases the IP addresses bound to all DHCP-enabled network adapters.<summary/>
        public uint ReleaseDHCPLeaseAll()
        {
            uint res = InvokeMethod("ReleaseDHCPLeaseAll", null, null);
            if (res == 1) rebootRequired = true;
            return res;
        }
        /// <summary> Renews the IP addresses on all DHCP-enabled network adapters.<summary/>
        public uint RenewDHCPLeaseAll()
        {
            uint res = InvokeMethod("RenewDHCPLeaseAll", null, null);
            if (res == 1) rebootRequired = true;
            return res;
        }
        /// <summary> Enables Ethernet packets to use 802.3 SNAP encoding.<summary/>
        public void SetArpUseEtherSNAP() { }
        /// <summary> Enables dead gateway detection.<summary/>
        public void SetDeadGWDetect() { }
        /// <summary> Sets the default Time to Live (TTL) value in the header of outgoing IP packets.<summary/>
        public void SetDefaultTTL() { }
        /// <summary> Sets the server search order as an array of elements.<summary/>
        public uint SetDNSServerSearchOrder(string[] dnsAddress)
        {
            if (dnsAddress == null) return uint.MaxValue;
            //if (dnsAddress.Count() == 0) return uint.MaxValue - 2;

            //Make sure to remove IPV6 addresses
            int cnt = dnsAddress.Count() - 1;
            for (int i = cnt; i >= 0; i--)
            {
                if (dnsAddress[i].Contains(":"))
                {
                    IList<string> ips = dnsAddress.ToList();
                    ips.RemoveAt(i);
                    dnsAddress = ips.ToArray();
                }
            }

            string method = "SetDNSServerSearchOrder";
            using (ManagementBaseObject par = _man.GetMethodParameters(method))
            {
                par.SetPropertyValue("DNSServerSearchOrder", dnsAddress);
                return InvokeMethod(method, par, null);
            }
        }
        /// <summary> Indicates dynamic DNS registration of IP addresses for this IP-bound adapter.<summary/>
        public uint SetDynamicDNSRegistration(bool fullDNSRegistrationEnabled, bool domainDNSRegistrationEnabled)
        {
            string method = "SetDynamicDNSRegistration";
            using (ManagementBaseObject par = _man.GetMethodParameters(method))
            {
                par.SetPropertyValue("FullDNSRegistrationEnabled", fullDNSRegistrationEnabled);
                par.SetPropertyValue("DomainDNSRegistrationEnabled", domainDNSRegistrationEnabled);
                return InvokeMethod(method, par, null);
            }

        }
        /// <summary> Specifies a list of gateways for routing packets destined for a different subnet than the one this adapter is connected to.<summary/>
        public uint SetGateways(string[] defaultIPGateway, ushort[] gatewayCostMetric)
        {
            if (defaultIPGateway == null) return uint.MaxValue;
            if (gatewayCostMetric == null) return uint.MaxValue - 1;
            if (defaultIPGateway.Count() == 0) return uint.MaxValue - 2;
            if (gatewayCostMetric.Count() == 0) return uint.MaxValue - 3;
            if (defaultIPGateway.Count() != gatewayCostMetric.Count()) return uint.MaxValue - 4;

            //Make sure to remove IPV6 addresses
            int cnt = defaultIPGateway.Count() - 1;
            for (int i = cnt; i >= 0; i--)
            {
                if (defaultIPGateway[i].Contains(":"))
                {
                    IList<string> ips = defaultIPGateway.ToList();
                    IList<ushort> snm = gatewayCostMetric.ToList();
                    ips.RemoveAt(i);
                    snm.RemoveAt(i);
                    defaultIPGateway = ips.ToArray();
                    gatewayCostMetric = snm.ToArray();
                }
            }

            string method = "SetGateways";
            using (ManagementBaseObject par = _man.GetMethodParameters(method))
            {
                par.SetPropertyValue("DefaultIPGateway", defaultIPGateway);
                par.SetPropertyValue("GatewayCostMetric", gatewayCostMetric);
                return InvokeMethod(method, par, null);
            }
        }
        /// <summary> Sets the routing metric associated with this IP-bound adapter.<summary/>
        public void SetIPConnectionMetric() { }
        /// <summary> Sets Internetworking Packet Exchange (IPX) network number/frame pairs for this network adapter.<summary/>
        public void SetIPXFrameTypeNetworkPairs() { }
        /// <summary> Sets the interval separating Keep Alive Retransmissions until a response is received.<summary/>
        public void SetKeepAliveInterval() { }
        /// <summary> Sets the default Maximum Transmission Unit (MTU) for a network interface.<summary/>
        public void SetMTU() { }
        /// <summary> Sets the number of IP packet headers allocated for the router packet queue.<summary/>
        public void SetNumForwardPackets() { }
        /// <summary> Enables Maximum Transmission Unit (MTU) discovery.<summary/>
        public void SetPMTUDiscovery() { }
        /// <summary> Sets the number of attempts TCP will retransmit a connect request before aborting.<summary/>
        public void SetTcpMaxConnectRetransmissions() { }
        /// <summary> Sets the maximum number of connections that TCP may have open simultaneously.<summary/>
        public void SetTcpNumConnections() { }
        /// <summary> Sets the maximum TCP Receive Window size offered by the system.<summary/>
        public void SetTcpWindowSize() { }
        /// <summary> Enables the Dynamic Host Configuration Protocol (DHCP) for service with this network adapter.<summary/>
        public uint EnableDHCP()
        {
            uint res = InvokeMethod("EnableDHCP", null, null);
            if (res == 1) rebootRequired = true;
            return res;
        }
        /// <summary> Enables IPsec globally across all IP-bound network adapters.<summary/>
        public void EnableIPFilterSec() { }
        /// <summary> Enables static TCP/IP addressing for the target network adapter.<summary/>
        public uint EnableStatic(string[] ipAddress, string[] subnetMask)
        {
            if (ipAddress == null) return uint.MaxValue;
            if (subnetMask == null) return uint.MaxValue - 1;
            if (ipAddress.Count() == 0) return uint.MaxValue - 2;
            if (subnetMask.Count() == 0) return uint.MaxValue - 3;
            if (ipAddress.Count() != subnetMask.Count()) return uint.MaxValue - 4;

            //Make sure to remove IPV6 addresses
            int cnt = ipAddress.Count() - 1;
            for (int i = cnt; i >= 0; i--)
            {
                if (ipAddress[i].Contains(":"))
                {
                    IList<string> ips = ipAddress.ToList();
                    IList<string> snm = subnetMask.ToList();
                    ips.RemoveAt(i);
                    snm.RemoveAt(i);
                    ipAddress = ips.ToArray();
                    subnetMask = snm.ToArray();
                }
            }

            string method = "EnableStatic";
            using (ManagementBaseObject par = _man.GetMethodParameters(method))
            {
                par.SetPropertyValue("IPAddress", ipAddress);
                par.SetPropertyValue("SubnetMask", subnetMask);
                return InvokeMethod(method, par, null);
            }
        }
        /// <summary> Releases the IP address bound to a specific DHCP-enabled network adapter.<summary/>
        public uint ReleaseDHCPLease()
        {
            uint res = InvokeMethod("ReleaseDHCPLease", null, null);
            if (res == 1) rebootRequired = true;
            return res;
        }
        /// <summary> Renews the IP address on specific DHCP-enabled network adapters.<summary/>
        public uint RenewDHCPLease()
        {
            uint res = InvokeMethod("RenewDHCPLease", null, null);
            if (res == 1) rebootRequired = true;
            return res;
        }
        /// <summary> Sets the transmission of ARP queries by the TCP/IP.<summary/>
        public void SetArpAlwaysSourceRoute() { }
        /// <summary> Sets the path to the standard Internet database files (HOSTS, LMHOSTS, NETWORKS, and PROTOCOLS).<summary/>
        public void SetDatabasePath() { }
        /// <summary> Obsolete. This method sets the default Type of Service (TOS) value in the header of outgoing IP packets.<summary/>
        public void SetDefaultTOS() { }
        /// <summary> Sets the DNS domain.<summary/>
        public uint SetDNSDomain(string dnsDomain)
        {
            uint res;
            string method = "SetDNSDomain";
            using (ManagementBaseObject par = _man.GetMethodParameters(method))
            {
                par.SetPropertyValue("DNSDomain", dnsDomain);
                res = InvokeMethod(method, par, null);
            }
            if (res == 1) rebootRequired = true;
            return res;
        }
        /// <summary> Sets the suffix search order as an array of elements.<summary/>
        public void SetDNSSuffixSearchOrder() { }
        /// <summary> Specifies how much memory IP allocates to store packet data in the router packet queue.<summary/>
        public void SetForwardBufferMemory() { }
        /// <summary> Sets the extent to which the system supports IP multicasting and participates in the Internet Group Management Protocol.<summary/>
        public uint SetIGMPLevel(byte igmpLevel)
        {
            uint res;
            string method = "SetIGMPLevel";
            using (ManagementBaseObject par = _man.GetMethodParameters(method))
            {
                par.SetPropertyValue("IGMPLevel", igmpLevel);
                res = InvokeMethod(method, par, null);
            }
            if (res == 1) rebootRequired = true;
            return res;
        }
        /// <summary> Sets IP zero broadcast usage.<summary/>
        public void SetIPUseZeroBroadcast() { }
        /// <summary> Sets the Internetworking Packet Exchange (IPX) virtual network number on the target computer system.<summary/>
        public void SetIPXVirtualNetworkNumber() { }
        /// <summary> Sets how often TCP attempts to verify that an idle connection is still available by sending a Keep Alive packet.<summary/>
        public void SetKeepAliveTime() { }
        /// <summary> Enables detection of Black Hole routers.<summary/>
        public void SetPMTUBHDetect() { }
        /// <summary> Sets the default operation of NetBIOS over TCP/IP.<summary/>
        public void SetTcpipNetbios() { }
        /// <summary> Sets the number of times TCP will retransmit an individual data segment before aborting the connection.<summary/>
        public void SetTcpMaxDataRetransmissions() { }
        /// <summary> Specifies whether TCP uses the RFC 1122 specification for urgent data, or the mode used by Berkeley Software Design (BSD) derived systems.<summary/>
        public void SetTcpUseRFC1122UrgentPointer() { }
        /// <summary> Sets the primary and secondary Windows Internet Naming Service (WINS) servers on this TCP/IP-bound network adapter.<summary/>
        public void SetWINSServer() { }
    }
}
