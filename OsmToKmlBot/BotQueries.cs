using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OsmToKmlBot
{
    class BotQueries
    {
        public static Telegram.Bot.Types.Update[] GetUpdates( int offset = 0 )
        {
            string result;
            using ( var webClient = new WebClient() )
            {
                webClient.QueryString.Add( "offset", offset.ToString() );
                result = webClient.DownloadString( "https://api.telegram.org/bot" + Config.Token + "/getUpdates" );

            }
            var res = JsonConvert.DeserializeObject<Telegram.Bot.Types.ApiResponse<Telegram.Bot.Types.Update[]>>( result ).ResultObject;
            return res;
        }

        public static string BuildQuery( string name, float lat, float lon )
        {
            string query;
            string path = Config.RulesFolder + name + ".txt";

            using ( var rd = new StreamReader( path ) )
                query = rd.ReadToEnd();

            var bbox = String.Format( "{0},{1},{2},{3}",
                ( lat - 0.01 ).ToString().Replace( ',', '.' ),
                ( lon - 0.01 ).ToString().Replace( ',', '.' ),
                ( lat + 0.01 ).ToString().Replace( ',', '.' ),
                ( lon + 0.01 ).ToString().Replace( ',', '.' ) );

            return query.Replace( @"{{bbox}}", bbox );
        }

        public async static Task SendFileAsync( long chatId, string kml, string filename )
        {
            var url = String.Format( "https://api.telegram.org/bot{0}/sendDocument", Config.Token );

            using ( var form = new MultipartFormDataContent() )
            {
                using ( Stream kmlStream = GenerateStreamFromString( kml ) )
                {
                    form.Add( new StringContent( chatId.ToString(), Encoding.UTF8 ), "chat_id" );
                    form.Add( new StreamContent( kmlStream ), "document", filename );

                    using ( var client = new HttpClient() )
                    {
                        await client.PostAsync( url, form );
                    }
                }
            }
        }

        static Stream GenerateStreamFromString( string s )
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter( stream );
            writer.Write( s );
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
