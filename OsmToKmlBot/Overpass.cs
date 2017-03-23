using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OsmToKmlBot
{
    public class Overpass
    {
        static readonly Uri OverpassApiUri = new Uri( "http://overpass-api.de/api/interpreter" );

        public static string RunQuery( string query )
        {
            string result;
            using ( var webClient = new WebClient() )
            {
                webClient.QueryString.Add( "data", query );
                result = webClient.DownloadString( OverpassApiUri );
            }
            return result;
        }

        public static async Task<string> RunQueryAsync( string query )
        {
            using ( var HTTPClient = new HttpClient() )
            {
                using ( var ResponseMessage = await HTTPClient.PostAsync( OverpassApiUri, new StringContent( query ) ) )
                {
                    if ( ResponseMessage.StatusCode == HttpStatusCode.OK )
                    {
                        using ( var ResponseContent = ResponseMessage.Content )
                        {
                            return await ResponseContent.ReadAsStringAsync();
                        }
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
        }
    }
}
