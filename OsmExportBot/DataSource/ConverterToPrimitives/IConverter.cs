using OsmExportBot.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsmExportBot.DataSource.ConverterToPrimitives
{
    public interface IConverter
    {
        PrimitiveCollections Convert(string raw, Query query);
    }
}
