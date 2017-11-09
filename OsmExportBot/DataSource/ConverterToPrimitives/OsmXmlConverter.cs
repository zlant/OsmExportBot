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
    public class OsmXmlConverter : IConverter
    {
        public static Line ConvertOsmWayToPrimitiveLine(osmWay way)
        {
            return new Line(way.nodes
                .Select(x => new Point(x.lat, x.lon)));
        }

        public PrimitiveCollections Convert(string xml, Query query)
        {
            var osm = Desir(xml);
            PreprocessingOsmXml(osm);
            return Convert(osm, query);
        }

        private void PreprocessingOsmXml(osm xml)
        {
            var nodes = xml.node
                .OrderBy(x => x.id)
                .ToArray();

            var ids = nodes
                .Select(x => x.id)
                .ToList();

            for (int i = 0; i < xml.way.Length; i++)
            {
                xml.way[i].nodes = xml.way[i].nd
                    .Select(x => nodes[ids.BinarySearch(x.@ref)])
                    .ToArray();

                for (int j = 0; j < xml.way[i].nodes.Length; j++)
                {
                    xml.way[i].nodes[j].refed = true;
                }
            }
        }

        protected virtual PrimitiveCollections Convert(osm xml, Query query)
        {
            var primitives = new PrimitiveCollections();

            var lines = xml.way
                .Select(x => ConvertOsmWayToPrimitiveLine(x));

            primitives.Lines.AddRange(lines);

            var unrefNodes = xml.node
                .Where(x => !x.refed)
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
