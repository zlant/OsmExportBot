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
        string[] stylesMapsMe = {
            "#placemark-blue",
            "#placemark-brown",
            "#placemark-green",
            "#placemark-orange",
            "#placemark-pink",
            "#placemark-purple",
            "#placemark-red",
            "#placemark-yellow"
        };

        Dictionary<string, Color32> colors = new Dictionary<string, Color32> {
            { "red",     new Color32(0x53, 0x00, 0x00, 0xab) },
            { "blue",    new Color32(0xb3, 0xdd, 0x22, 0x00) },
            { "green",   new Color32(0xb3, 0x00, 0xff, 0x00) },
            { "orange",  new Color32(0xb3, 0x00, 0xa5, 0xff) },
            { "gray",    new Color32(0x88, 0x88, 0x88, 0x88) },
            { "purpure", new Color32(0xb3, 0x80, 0x00, 0x80) }
        };

        public override string FileExtension { get; set; } = ".kml";

        public override string Generate(PrimitiveCollections primitives, string fileName)
        {
            var placemarks = new List<Placemark>();

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
                    StyleUrl = new Uri("#" + coord.Color, UriKind.Relative)
                };
                placemarks.Add(placemark);
            }

            foreach (var line in primitives.Lines)
            {
                var placemark = new Placemark {
                    Geometry = PrimitiveLineToLineString(line),
                    StyleUrl = new Uri("#" + line.Color, UriKind.Relative)
                };
                placemarks.Add(placemark);
            }

            return GenerateKmlFromFeatures(placemarks, fileName);
        }

        private LineString PrimitiveLineToLineString(Line line)
        {
            var vectors = line.Points
                .Select(p => new Vector { Latitude = p.Lat, Longitude = p.Lon });

            return new LineString {
                Coordinates = new CoordinateCollection(vectors)
            };
        }

        private string GenerateKmlFromFeatures(IEnumerable<Feature> features, string filename)
        {
            var document = new Document();
            document.Name = filename;

            foreach (var color in colors)
            {
                Style style = new Style();
                style.Id = color.Key;
                style.Line = new LineStyle();
                style.Line.Color = color.Value;

                document.AddStyle(style);
            }

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
