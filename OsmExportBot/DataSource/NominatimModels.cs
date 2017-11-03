using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OsmExportBot.DataSource
{
    [XmlType(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class reversegeocode
    {
        public reversegeocodeResult result { get; set; }
        
        public reversegeocodeAddressparts addressparts { get; set; }
        
        [XmlAttribute()]
        public string timestamp { get; set; }
        
        [XmlAttribute()]
        public string querystring { get; set; }
    }
    
    [XmlType(AnonymousType = true)]
    public partial class reversegeocodeResult
    {
        [XmlAttribute()]
        public uint place_id { get; set; }


        [XmlAttribute()]
        public string osm_type { get; set; }


        [XmlAttribute()]
        public uint osm_id { get; set; }


        [XmlText()]
        public string Value { get; set; }
    }
    
    [XmlType(AnonymousType = true)]
    public partial class reversegeocodeAddressparts
    {
        public byte house_number { get; set; }
        
        public string road { get; set; }
        
        public string village { get; set; }
        
        public string town { get; set; }
        
        public string city { get; set; }

        public string county { get; set; }

        public string postcode { get; set; }
        
        public string country { get; set; }
        
        public string country_code { get; set; }
    }


}
