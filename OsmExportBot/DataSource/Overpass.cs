using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OsmExportBot.DataSource
{
    public class OverPpass : IDataSource
    {
        public Uri OverpassApiUri { get; }

        public OverPpass() : this("http://overpass-api.de/api/interpreter")
        { }

        public OverPpass(string uri)
        {
            OverpassApiUri = new Uri(uri);
        }

        public string BuildQuery(string name, float lat, float lon)
        {
            var query = Rules.GetRule(name);

            var bbox = String.Format("{0},{1},{2},{3}",
                (lat - 0.01).ToString().Replace(',', '.'),
                (lon - 0.01).ToString().Replace(',', '.'),
                (lat + 0.01).ToString().Replace(',', '.'),
                (lon + 0.01).ToString().Replace(',', '.'));

            return query.Replace(@"{{bbox}}", bbox);
        }

        public string RunQuery(string query)
        {
            string result;
            using (var webClient = new WebClient())
            {
                webClient.QueryString.Add("data", query);
                result = webClient.DownloadString(OverpassApiUri);
            }
            return result;
        }
    }
}
