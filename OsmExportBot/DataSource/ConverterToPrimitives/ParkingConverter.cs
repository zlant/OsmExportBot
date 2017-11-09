using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsmExportBot.Primitives;

namespace OsmExportBot.DataSource.ConverterToPrimitives
{
    public class ParkingConverter : OsmXmlConverter
    {
        protected override PrimitiveCollections Convert(osm xml, Query query)
        {
            var primitives = new PrimitiveCollections();

            var pways = xml.way
                .Where(x => ParkingLane.IsParkingLane(x))
                .SelectMany(x => new ParkingLane(x).GetLines());

            primitives.Lines.AddRange(pways);

            var lines = xml.way
                 .Where(x => !ParkingLane.IsParkingLane(x))
                 .Select(x => ConvertParkingPoly(x))
                 .ToList();

            primitives.Lines.AddRange(lines);

            var unrefNodes = xml.node
                .Where(x => !x.refed)
                .Select(x => ConvertParkingNode(x));

            primitives.Points.AddRange(unrefNodes);

            return primitives;
        }

        private Line ConvertParkingPoly(osmWay way)
        {
            var line = ConvertOsmWayToPrimitiveLine(way);
            line.Color = GetColor(way);
            return line;
        }

        private Point ConvertParkingNode(osmNode node)
        {
            var color = GetColor(node);
            return new Point(node.lat, node.lon, color);
        }

        private string GetColor(osmGeo parking)
        {
            var access = parking.tag.FirstOrDefault(x => x.k == "access")?.v;
            var fee = parking.tag.FirstOrDefault(x => x.k == "fee")?.v;

            switch (access)
            {
                case "private": return "gray";
                case "customers":
                case "residents": return "purpure";
            }

            switch (fee)
            {
                case "yes": return "blue";
                default: return "green";
            }
        }
    }
}
