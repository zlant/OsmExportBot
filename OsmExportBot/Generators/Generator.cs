using OsmExportBot.Primitives;
using System;

namespace OsmExportBot.Generators
{
    public abstract class Generator
    {
        public abstract string FileExtension { get; set; }

        public abstract string Generate(PrimitiveCollections primitives, string fileName);

        public string GenerateFileName(string rule)
        {
            return $"{rule}_{DateTime.Now.ToString("yyMMdd_HHmm")}{FileExtension}";
        }
    }
}
