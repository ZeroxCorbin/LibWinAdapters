﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LibWinPhyAdapters.Manager
{
    public class AdapterXMLFile
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

            }

            public string AdapterType
            {
                get; set;
            }
            public ushort AdapterTypeID
            {
                get; set;
            }
            public bool AutoSense
            {
                get; set;
            }
            public ushort Availability
            {
                get; set;
            }
            public string Availability_String
            {
                get; set;
            }
            public string Caption
            {
                get; set;
            }
            public uint ConfigManagerErrorCode
            {
                get; set;
            }
            public bool ConfigManagerUserConfig
            {
                get; set;
            }
            public string CreationClassName
            {
                get; set;
            }
            public string Description
            {
                get; set;
            }
            public string DeviceID
            {
                get; set;
            }
            public bool ErrorCleared
            {
                get; set;
            }
            public string ErrorDescription
            {
                get; set;
            }
            public string GUID
            {
                get; set;
            }
            public uint Index
            {
                get; set;
            }
            public DateTime InstallDate
            {
                get; set;
            }
            public bool Installed
            {
                get; set;
            }
            public uint InterfaceIndex
            {
                get; set;
            }
            public uint LastErrorCode
            {
                get; set;
            }
            public string MACAddress
            {
                get; set;
            }
            public string Manufacturer
            {
                get; set;
            }
            public uint MaxNumberControlled
            {
                get; set;
            }
            public ulong MaxSpeed
            {
                get; set;
            }
            public string Name
            {
                get; set;
            }
            public string NetConnectionID
            {
                get; set;
            }
            public ushort NetConnectionStatus
            {
                get; set;
            }
            public string NetConnectionStatus_String
            {
                get; set;
            }
            public bool NetEnabled
            {
                get; set;
            }
            [XmlArrayItem(IsNullable = false)]
            public string[] NetworkAddresses
            {
                get; set;
            }
            public string PermanentAddress
            {
                get; set;
            }
            public bool PhysicalAdapter
            {
                get; set;
            }
            public string PNPDeviceID
            {
                get; set;
            }
            [XmlArrayItem(IsNullable = false)]
            public ushort[] PowerManagementCapabilities
            {
                get; set;
            }
            public string PowerManagementCapabilities_String
            {
                get; set;
            }
            public bool PowerManagementSupported
            {
                get; set;
            }
            public string ProductName
            {
                get; set;
            }
            public string ServiceName
            {
                get; set;
            }
            public ulong Speed
            {
                get; set;
            }
            public string Status
            {
                get; set;
            }
            public ushort StatusInfo
            {
                get; set;
            }
            public string StatusInfo_String
            {
                get; set;
            }
            public string SystemCreationClassName
            {
                get; set;
            }
            public string SystemName
            {
                get; set;
            }
            public DateTime TimeOfLastReset
            {
                get; set;
            }
        }
    }
}
