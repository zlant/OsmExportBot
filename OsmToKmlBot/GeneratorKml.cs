using SharpKml.Base;
using SharpKml.Dom;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsmToKmlBot
{
    public class GeneratorKml
    {
        static string[] colours = {
            "#placemark-blue",
            "#placemark-brown",
            "#placemark-green",
            "#placemark-orange",
            "#placemark-pink",
            "#placemark-purple",
            "#placemark-red",
            "#placemark-yellow"
        };

        public static string GenerateKml( string coords, string filename, int colour = 6 )
        {
            var placemarks = new Document();
            placemarks.Name = filename;

            foreach ( var line in coords.Split( '\n' ) )
            {
                var location = line.Trim().Split( '\t' );
                double lat, lon;

                if ( !double.TryParse( location[ 0 ], NumberStyles.Float, CultureInfo.InvariantCulture, out lat )
                    || !double.TryParse( location[ 1 ], NumberStyles.Float, CultureInfo.InvariantCulture, out lon ) )
                    continue;

                var point = new Point();
                point.Coordinate = new Vector( lat, lon );

                var style = new Uri( colours[ colour % colours.Length ], UriKind.Relative );

                Placemark placemark = new Placemark();
                placemark.Geometry = point;
                placemark.StyleUrl = style;

                placemarks.AddFeature( placemark );
            }

            Kml kml = new Kml();
            kml.Feature = placemarks;

            Serializer serializer = new Serializer();
            serializer.Serialize( kml );

            return serializer.Xml;
        }
    }
}
