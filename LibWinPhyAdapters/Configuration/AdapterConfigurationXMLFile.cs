using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LibWinPhyAdapters.Configuration
{
    public class AdapterConfigurationXMLFile
    {
        public static XMLRoot Load(string file)
        {
            StreamReader sr;
            XMLRoot app;
            XmlSerializer serializer = new XmlSerializer(typeof(XMLRoot));
            try
            {
                sr = new StreamReader(file);
            }
            catch (FileNotFoundException)
            {
                Save(file, new XMLRoot());
                sr = new StreamReader(file);
            }

            app = (XMLRoot)serializer.Deserialize(sr);
            sr.Close();
            return app;
        }

        public static void Save(string file, XMLRoot app)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(XMLRoot));
            using (StreamWriter sw = new StreamWriter(file))
            {
                serializer.Serialize(sw, app);
            }
        }

        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [Serializable()]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true, Namespace = "http://InterManage.com")]
        [XmlRoot(Namespace = "http://InterManage.com", IsNullable = false)]
        public partial class XMLRoot
        {
            public XMLRoot() { }
            public XMLRoot(bool init = true)
            {
                if (!init) return;

                DefaultIPGateway = new string[0];
                DNSDomainSuffixSearchOrder = new string[0]; ;
                DNSServerSearchOrder = new string[0]; ;
                IPAddress = new string[0]; ;
                IPSecPermitIPProtocols = new string[0]; ;
                IPSecPermitTCPPorts = new string[0]; ;
                IPSecPermitUDPPorts = new string[0]; ;
                IPSubnet = new string[0]; ;
                IPXNetworkNumber = new string[0]; ;

                GatewayCostMetric = new ushort[0];
                IPXFrameType = new uint[0];
            }

            public string Caption
            {
                get; set;
            }
            public string Description
            {
                get; set;
            }
            public string SettingID
            {
                get; set;
            }
            public bool ArpAlwaysSourceRoute
            {
                get; set;
            }
            public bool ArpUseEtherSNAP
            {
                get; set;
            }
            public string DatabasePath
            {
                get; set;
            }
            public bool DeadGWDetectEnabled
            {
                get; set;
            }
            [XmlArrayItem(IsNullable = false)]
            public string[] DefaultIPGateway
            {
                get; set;
            }
            public uint DefaultTOS
            {
                get; set;
            }
            public uint DefaultTTL
            {
                get; set;
            }
            public bool DHCPEnabled
            {
                get; set;
            }
            public DateTime DHCPLeaseExpires
            {
                get; set;
            }
            public DateTime DHCPLeaseObtained
            {
                get; set;
            }
            public string DHCPServer
            {
                get; set;
            }
            public string DNSDomain
            {
                get; set;
            }
            [XmlArrayItem(IsNullable = false)]
            public string[] DNSDomainSuffixSearchOrder
            {
                get; set;
            }
            public bool DNSEnabledForWINSResolution
            {
                get; set;
            }
            public string DNSHostName
            {
                get; set;
            }
            [XmlArrayItem(IsNullable = false)]
            public string[] DNSServerSearchOrder
            {
                get; set;
            }
            public bool DomainDNSRegistrationEnabled
            {
                get; set;
            }
            public uint ForwardBufferMemory
            {
                get; set;
            }
            public bool FullDNSRegistrationEnabled
            {
                get; set;
            }
            [XmlArrayItem(IsNullable = false)]
            public ushort[] GatewayCostMetric
            {
                get; set;
            }
            public uint IGMPLevel
            {
                get; set;
            }
            public uint Index
            {
                get; set;
            }
            public uint InterfaceIndex
            {
                get; set;
            }
            [XmlArrayItem(IsNullable = false)]
            public string[] IPAddress
            {
                get; set;
            }
            public uint IPConnectionMetric
            {
                get; set;
            }
            public bool IPEnabled
            {
                get; set;
            }
            public bool IPFilterSecurityEnabled
            {
                get; set;
            }
            public bool IPPortSecurityEnabled
            {
                get; set;
            }
            [XmlArrayItem(IsNullable = false)]
            public string[] IPSecPermitIPProtocols
            {
                get; set;
            }
            [XmlArrayItem(IsNullable = false)]
            public string[] IPSecPermitTCPPorts
            {
                get; set;
            }
            [XmlArrayItem(IsNullable = false)]
            public string[] IPSecPermitUDPPorts
            {
                get; set;
            }
            [XmlArrayItem(IsNullable = false)]
            public string[] IPSubnet
            {
                get; set;
            }
            public bool IPUseZeroBroadcast
            {
                get; set;
            }
            public string IPXAddress
            {
                get; set;
            }
            public bool IPXEnabled
            {
                get; set;
            }
            [XmlArrayItem(IsNullable = false)]
            public uint[] IPXFrameType
            {
                get; set;
            }
            public uint IPXMediaType
            {
                get; set;
            }
            [XmlArrayItem(IsNullable = false)]
            public string[] IPXNetworkNumber
            {
                get; set;
            }
            public string IPXVirtualNetNumber
            {
                get; set;
            }
            public uint KeepAliveInterval
            {
                get; set;
            }
            public uint KeepAliveTime
            {
                get; set;
            }
            public string MACAddress
            {
                get; set;
            }
            public uint MTU
            {
                get; set;
            }
            public uint NumForwardPackets
            {
                get; set;
            }
            public bool PMTUBHDetectEnabled
            {
                get; set;
            }
            public bool PMTUDiscoveryEnabled
            {
                get; set;
            }
            public string ServiceName
            {
                get; set;
            }
            public uint TcpipNetbiosOptions
            {
                get; set;
            }
            public uint TcpMaxConnectRetransmissions
            {
                get; set;
            }
            public uint TcpMaxDataRetransmissions
            {
                get; set;
            }
            public uint TcpNumConnections
            {
                get; set;
            }
            public bool TcpUseRFC1122UrgentPointer
            {
                get; set;
            }
            public ushort TcpWindowSize
            {
                get; set;
            }
            public bool WINSEnableLMHostsLookup
            {
                get; set;
            }
            public string WINSHostLookupFile
            {
                get; set;
            }
            public string WINSPrimaryServer
            {
                get; set;
            }
            public string WINSScopeID
            {
                get; set;
            }
            public string WINSSecondaryServer
            {
                get; set;
            }
        }
    }
}
