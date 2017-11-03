using OsmExportBot.Primitives;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OsmExportBot.DataSource
{
    [Flags]
    enum ParseOption
    {
        NoOption = 0,
        ParkingLaneShift = 1
    }

    public class Overpass
    {
        public Uri ApiUri { get; }

        public Overpass() : this("http://overpass-api.de/api/interpreter")
        { }

        public Overpass(string uri)
        {
            ApiUri = new Uri(uri);
        }

        #region Builder query
        public string BuildQuery(string name, float lat, float lon)
        {
            var request = Rules.GetRule(name);

            if (request.Contains(@"{{bbox}}"))
            {
                var bbox = GetBbox(lat, lon);
                request = request.Replace(@"{{bbox}}", bbox);
            }
            else
            {
                var nominatim = new Nominatim();
                var cityId = nominatim.GetPlaceId(lat, lon);
                request = request.Replace(@"{{city}}", cityId);
            }

            return request;
        }

        private string GetBbox(float lat, float lon)
        {
            return String.Format("{0},{1},{2},{3}",
                (lat - 0.01).ToString().Replace(',', '.'),
                (lon - 0.01).ToString().Replace(',', '.'),
                (lat + 0.01).ToString().Replace(',', '.'),
                (lon + 0.01).ToString().Replace(',', '.'));
        }
        #endregion

        #region Runner query
        public PrimitiveCollections RunQuery(string query)
        {
            string result;
            using (var webClient = new WebClient())
            {
                webClient.QueryString.Add("data", query);
                result = webClient.DownloadString(ApiUri);
            }

            if (IsXmlResponse(query))
                return ConvertToPrimitives(Desir<osm>(result), GetParsingOption(query));
            else
                return ConvertToPrimitives(result);
        }

        private T Desir<T>(string xml)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(T));
            T result;

            using (TextReader streamReader = new StringReader(xml))
            {
                result = (T)formatter.Deserialize(streamReader);
            }

            return result;
        }

        public bool IsXmlResponse(string request)
        {
            return request.Contains("out:xml");
        }

        private ParseOption GetParsingOption(string query)
        {
            ParseOption options = ParseOption.NoOption;

            if (query.Contains("parking:lane:"))
                options = options | ParseOption.ParkingLaneShift;

            return options;
        }

        private PrimitiveCollections ConvertToPrimitives(string csv)
        {
            var primitives = new PrimitiveCollections();

            foreach (var line in csv.Split('\n'))
            {
                var location = line.Trim().Split('\t');
                double lat, lon;

                if (!double.TryParse(location[0], NumberStyles.Float, CultureInfo.InvariantCulture, out lat) ||
                    !double.TryParse(location[1], NumberStyles.Float, CultureInfo.InvariantCulture, out lon))
                    continue;

                primitives.Points.Add(new Point(lat, lon));
            }

            return primitives;
        }

        private PrimitiveCollections ConvertToPrimitives(osm xml, ParseOption options)
        {
            var primitives = new PrimitiveCollections();

            xml.node = xml.node
                .OrderBy(x => x.id)
                .ToArray();

            if (options == ParseOption.ParkingLaneShift)
            {
                var pways = xml.way
                    .Where(x => ParkingLanes.IsParkingLane(x))
                    .Select(x => new ParkingLanes(x, xml.node))
                    .SelectMany(x => x.GetLines());

                primitives.Lines.AddRange(pways);
            }

            var ways = xml.way
                .Where(x => !ParkingLanes.IsParkingLane(x))
                .Select(x => OverpassModelsConverter.ConvertOsmWayToPrimitiveLine(x, xml.node));

            primitives.Lines.AddRange(ways);


            var unrefNodes = xml.node
                .Select(x => x.id)
                .Except(xml.way
                    .SelectMany(x => x.nd.Select(z => z.@ref)))
                .Join(xml.node, o => o, i => i.id, (o, i) => i)
                .Select(x => new Point(x.lat, x.lon));

            primitives.Points.AddRange(unrefNodes);

            return primitives;
        }
        #endregion
    }
}
