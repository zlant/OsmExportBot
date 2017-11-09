using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OsmExportBot.DataSource
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public partial class osm
    {
        public osmMeta meta { get; set; }
        
        [XmlElement("way")]
        public osmWay[] way { get; set; }
        
        [XmlElement("node")]
        public osmNode[] node { get; set; }
        
        [XmlAttribute()]
        public decimal version { get; set; }
        
        [XmlAttribute()]
        public string generator { get; set; }
    }
    
    [XmlType(AnonymousType = true)]
    public partial class osmMeta
    {
        [XmlAttribute()]
        public System.DateTime osm_base { get; set; }
    }

    [XmlType(AnonymousType = true)]
    public partial class osmGeo
    {
        [XmlElement("tag")]
        public osmTag[] tag { get; set; }

        [XmlAttribute()]
        public ulong id { get; set; }
    }
    
    [XmlType(AnonymousType = true)]
    public partial class osmWay : osmGeo
    {
        [XmlIgnore()]
        public osmNode[] nodes { get; set; }

        [XmlElement("nd")]
        public osmWayND[] nd { get; set; }
        /*
        [XmlElement("tag")]
        public osmTag[] tag { get; set; }
        
        [XmlAttribute()]
        public ulong id { get; set; }*/
    }
    
    [XmlType(AnonymousType = true)]
    public partial class osmWayND
    {
        [XmlAttribute()]
        public ulong @ref { get; set; }
    }
    
    [XmlType(AnonymousType = true)]
    public partial class osmTag
    {
        [XmlAttribute()]
        public string k { get; set; }
        
        [XmlAttribute()]
        public string v { get; set; }
    }
    
    [XmlType(AnonymousType = true)]
    public partial class osmNode : osmGeo
    {/*
        [XmlAttribute()]
        public ulong id { get; set; }*/
        
        [XmlAttribute()]
        public double lat { get; set; }
        
        [XmlAttribute()]
        public double lon { get; set; }
        /*
        [XmlElement("tag")]
        public osmTag[] tag { get; set; }*/

        [XmlIgnore()]
        public bool refed { get; set; }
    }
}
