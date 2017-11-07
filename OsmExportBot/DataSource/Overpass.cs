using OsmExportBot.DataSource.ConverterToPrimitives;
using OsmExportBot.Generators;
using OsmExportBot.Primitives;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OsmExportBot.DataSource
{
    [Flags]
    enum ParseOption
    {
        NoOption = 0,
        ParkingLaneShift = 1,
        ParkingFree = 2,
        Parking = 4
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
        public void BuildQuery(Query query, float lat, float lon)
        {
            var request = Rules.GetRule(query.RuleName);

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

            query.Request = request;
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
        public void RunQuery(Query query)
        {
            string result;
            using (var webClient = new WebClient())
            {
                webClient.QueryString.Add("data", query.Request);
                result = webClient.DownloadString(ApiUri);
            }

            IConverter converet = GetConverter(query);
            query.Response = converet.Convert(result, query);
        }

        private IConverter GetConverter(Query query)
        {
            if (query.IsXmlResponse())
            {
                var options = GetParsingOption(query);

                if (options.HasFlag(ParseOption.ParkingLaneShift))
                {
                    return new ParkingLanesConvertor();
                }
                else if (options.HasFlag(ParseOption.ParkingFree))
                {
                    return new ParkingFreeConverter();
                }
                else return new OsmXmlCoverter();
            }
            else // if csv
            {
                return new CsvConverter();
            }
        }

        private ParseOption GetParsingOption(Query query)
        {
            ParseOption options = ParseOption.NoOption;

            if (query.Request.Contains("parking:lane:") || query.Request.Contains("parking:condition:"))
                options = options | ParseOption.ParkingLaneShift;

            if (query.RuleName.Contains("free") && query.RuleName.Contains("parking"))
                options = options | ParseOption.ParkingFree;

            if (query.RuleName.Contains("parking"))
                options = options | ParseOption.Parking;

            return options;
        }
        #endregion
    }
}
