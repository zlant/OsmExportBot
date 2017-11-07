using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsmExportBot.Primitives;
using System.Xml.Serialization;
using System.IO;

namespace OsmExportBot.DataSource.ConverterToPrimitives
{
    public class OsmXmlCoverter : IConverter
    {
        public static Line ConvertOsmWayToPrimitiveLine(osmWay way, osmNode[] nodes)
        {
            var ids = nodes
                .Select(x => x.id)
                .ToList();

            return new Line(way.nd
                .Select(x => nodes[ids.BinarySearch(x.@ref)])
                .Select(x => new Point(x.lat, x.lon)));
        }

        public PrimitiveCollections Convert(string xml, Query query)
        {
            var osm = Desir(xml);
            return Convert(osm, query);
        }

        protected virtual PrimitiveCollections Convert(osm xml, Query query)
        {
            var primitives = new PrimitiveCollections();

            var nodes = xml.node
                .OrderBy(x => x.id)
                .ToArray();
            
            var lines = xml.way
                .Select(x => ConvertOsmWayToPrimitiveLine(x, nodes));

            primitives.Lines.AddRange(lines);

            var unrefNodes = xml.node
                .Select(x => x.id)
                .Except(xml.way
                    .SelectMany(x => x.nd.Select(z => z.@ref)))
                .Join(xml.node, o => o, i => i.id, (o, i) => i)
                .Select(x => new Point(x.lat, x.lon));

            primitives.Points.AddRange(unrefNodes);
            
            return primitives;
        }

        private osm Desir(string xml)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(osm));
            osm result;

            using (TextReader streamReader = new StringReader(xml))
            {
                result = (osm)formatter.Deserialize(streamReader);
            }

            return result;
        }
    }
}
