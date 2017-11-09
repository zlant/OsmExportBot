using OsmExportBot.DataSource;
using OsmExportBot.Primitives;
using SharpKml.Base;
using SharpKml.Dom;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsmExportBot.Generators
{
    public class GeneratorKml : Generator
    {
        public static string[] StylesMapsMe = {
            "blue",
            "brown",
            "green",
            "orange",
            "pink",
            "purple",
            "red",
            "yellow"
        };

        Dictionary<string, Color32> colors = new Dictionary<string, Color32> {
            { "red",     new Color32(0x53, 0x00, 0x00, 0xab) },
            { "blue",    new Color32(0xb3, 0xdd, 0x22, 0x00) },
            { "green",   new Color32(0xb3, 0x00, 0xe0, 0x00) },
            { "orange",  new Color32(0xb3, 0x00, 0xa5, 0xff) },
            { "gray",    new Color32(0x88, 0x88, 0x88, 0x88) },
            { "purpure", new Color32(0xb3, 0x80, 0x00, 0x80) }
        };

        public override string FileExtension { get; set; } = ".kml";

        public override string Generate(PrimitiveCollections primitives, string fileName)
        {
            var placemarks = new List<Placemark>();
            var styles = new List<Style>();

            foreach (var coord in primitives.Points)
            {
                var vector = new Vector {
                    Latitude = coord.Lat,
                    Longitude = coord.Lon
                };
                var point = new SharpKml.Dom.Point {
                    Coordinate = vector
                };
                var placemark = new Placemark {
                    Geometry = point,
                    StyleUrl = new Uri("#placemark-" + coord.Color, UriKind.Relative)
                };
                placemarks.Add(placemark);
            }

            if (primitives.Lines.Count > 0)
                foreach (var color in colors)
                {
                    Style style = new Style();
                    style.Id = color.Key;
                    style.Line = new LineStyle();
                    style.Line.Color = color.Value;

                    styles.Add(style);
                }

            foreach (var line in primitives.Lines)
            {
                var placemark = new Placemark {
                    Geometry = PrimitiveLineToLineString(line),
                    StyleUrl = new Uri("#" + line.Color, UriKind.Relative)
                };
                placemarks.Add(placemark);
            }

            return GenerateKmlFromFeatures(placemarks, styles, fileName);
        }

        private LineString PrimitiveLineToLineString(Line line)
        {
            var vectors = line.Points
                .Select(p => new Vector { Latitude = p.Lat, Longitude = p.Lon });

            return new LineString {
                Coordinates = new CoordinateCollection(vectors)
            };
        }

        private string GenerateKmlFromFeatures(IEnumerable<Feature> features, IEnumerable<Style> styles, string filename)
        {
            var document = new Document();
            document.Name = filename;

            foreach (var style in styles)
                document.AddStyle(style);

            foreach (var feature in features)
                document.AddFeature(feature);

            Kml kml = new Kml();
            kml.Feature = document;

            Serializer serializer = new Serializer();
            serializer.Serialize(kml);

            return serializer.Xml;
        }
    }
}
