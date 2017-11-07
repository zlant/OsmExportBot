using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsmExportBot.Primitives;

namespace OsmExportBot.DataSource.ConverterToPrimitives
{
    class ParkingFreeConverter : OsmXmlCoverter
    {
        protected override PrimitiveCollections Convert(osm xml, Query query)
        {
            var primitives = new PrimitiveCollections();

            var nodes = xml.node
                .OrderBy(x => x.id)
                .ToArray();

            var ways = xml.way.ToList();

            var pways = xml.way
                .Where(x => ParkingLanes.IsParkingLane(x))
                .Select(x => new ParkingLanes(x, nodes))
                .SelectMany(x => x.GetLines());

            primitives.Lines.AddRange(pways);

            primitives.Lines = primitives.Lines
                .Where(x => x.Color == "green")
                .ToList();

            ways = ways
                .Except(ways.Where(x => ParkingLanes.IsParkingLane(x)))
                .ToList();

            var lines = ways
                //.Where(x => !ParkingLanes.IsParkingLane(x))
                .Select(x => ConvertOsmWayToPrimitiveLine(x, nodes));

            primitives.Lines.AddRange(lines);

            var unrefNodes = xml.node
                .Select(x => x.id)
                .Except(xml.way
                    .SelectMany(x => x.nd.Select(z => z.@ref)))
                .Join(xml.node, o => o, i => i.id, (o, i) => i)
                .Select(x => new Point(x.lat, x.lon));

            primitives.Points.AddRange(unrefNodes);

            primitives.Lines = primitives.Lines
                .Select(x => new Line(x.Points, "green"))
                .ToList();

            primitives.Points = primitives.Points
                .Select(x => new Point(x.Lat, x.Lon, "green"))
                .ToList();

            return primitives;
        }
    }
}
