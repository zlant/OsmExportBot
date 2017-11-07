using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsmExportBot.Primitives;

namespace OsmExportBot.DataSource.ConverterToPrimitives
{
    class ParkingLanesConvertor : OsmXmlCoverter
    {
        protected override PrimitiveCollections Convert(osm xml, Query query)
        {
            var primitives = new PrimitiveCollections();

            var nodes = xml.node
                .OrderBy(x => x.id)
                .ToArray();

            var pways = xml.way
                .Where(x => ParkingLanes.IsParkingLane(x))
                .Select(x => new ParkingLanes(x, nodes))
                .SelectMany(x => x.GetLines());
            
            primitives.Lines.AddRange(pways);

            return primitives;
        }
    }
}
