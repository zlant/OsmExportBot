using OsmExportBot.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsmExportBot.DataSource
{
    static class OverpassModelsConverter
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
    }
}
