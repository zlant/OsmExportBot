using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsmExportBot.Primitives;
using System.Globalization;
using OsmExportBot.Generators;

namespace OsmExportBot.DataSource.ConverterToPrimitives
{
    class CsvConverter : IConverter
    {
        public PrimitiveCollections Convert(string csv, Query query)
        {
            var color = GeneratorKml.StylesMapsMe[Rules.IndexOfRule(query.RuleName) % GeneratorKml.StylesMapsMe.Length];

            var primitives = new PrimitiveCollections();

            foreach (var line in csv.Split('\n'))
            {
                var location = line.Trim().Split('\t');
                double lat, lon;

                if (!double.TryParse(location[0], NumberStyles.Float, CultureInfo.InvariantCulture, out lat) ||
                    !double.TryParse(location[1], NumberStyles.Float, CultureInfo.InvariantCulture, out lon))
                    continue;

                primitives.Points.Add(new Point(lat, lon, color));
            }

            return primitives;
        }
    }
}
