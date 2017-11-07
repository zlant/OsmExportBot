using OsmExportBot.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsmExportBot.DataSource
{
    public class Query
    {
        public string RuleName { get; set; }
        public string Request { get; set; }
        public PrimitiveCollections Response { get; set; }

        public bool IsXmlResponse()
        {
            return Request.Contains("out:xml");
        }
    }
}
