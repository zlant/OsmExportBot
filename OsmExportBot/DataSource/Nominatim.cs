using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OsmExportBot.DataSource
{
    class Nominatim
    {
        public Uri ApiUri { get; }

        public Nominatim() : this("http://nominatim.openstreetmap.org/reverse?")
        { }

        public Nominatim(string uri)
        {
            ApiUri = new Uri(uri);
        }

        public string BuildQuery(float lat, float lon)
        {
            return String.Format("{0},format=xml&lat={1}&lon={2}&zoom=10&addressdetails=1",
                ApiUri,
                lat.ToString().Replace(',', '.'),
                lon.ToString().Replace(',', '.'));
        }

        public string RunQuery(string query)
        {
            string result;
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add(HttpRequestHeader.UserAgent, "OsmExportBot");
                webClient.Headers.Add(HttpRequestHeader.AcceptLanguage, "en");
                result = webClient.DownloadString(query);
            }
            return result;
        }


        public string GetPlaceId(float lat, float lon)
        {
            var query = BuildQuery(lat, lon);
            var result = RunQuery(query);
            var desir = Desir<reversegeocode>(result);

            if (desir.result.osm_type == "relation")
                return (3600000000 + desir.result.osm_id).ToString();
            else if (desir.result.osm_type == "way")
                return (2400000000 + desir.result.osm_id).ToString();
            else
                return "";
        }

        T Desir<T>(string xml)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(T));
            T result;

            using (TextReader streamReader = new StringReader(xml))
            {
                result = (T)formatter.Deserialize(streamReader);
            }

            return result;
        }
    }
}
