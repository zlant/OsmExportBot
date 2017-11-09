using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsmExportBot.Primitives;

namespace OsmExportBot.DataSource.ConverterToPrimitives
{
    class ParkingFreeConverter : ParkingConverter
    {
        protected override PrimitiveCollections Convert(osm xml, Query query)
        {
            var primitives = base.Convert(xml, query);

            primitives.Lines = primitives.Lines
                .Where(x => x.Color == "green")
                .ToList();

            return primitives;
        }
    }
}
