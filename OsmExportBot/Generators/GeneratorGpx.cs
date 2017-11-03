using SharpKml.Base;
using SharpKml.Dom;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsmExportBot.DataSource;
using OsmExportBot.Primitives;

namespace OsmExportBot.Generators
{
    public class GeneratorGpx : Generator
    {
        public override string FileExtension { get; set; } = ".gpx";

        public override string Generate(PrimitiveCollections primitives, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
