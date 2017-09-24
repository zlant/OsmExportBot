using OsmExportBot.DataSource;
using OsmExportBot.Generators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace OsmExportBot.Commands
{
    public class LocationCommand : Command
    {
        public override string Name { get; set; }

        public override MessageType Type { get; set; } = MessageType.LocationMessage;

        private IDataSource overpass = new OverPpass();

        private Generator generatorKml = new GeneratorKMml();

        public override void Excecute(Message message, TelegramBotClient bot)
        {
            string rule = "nolvl";
            UserState.CurrentRule.TryRemove(message.Chat.Id, out rule);

            var query = overpass.BuildQuery(rule, message.Location.Latitude, message.Location.Longitude);

            var result = overpass.RunQuery(query);
            var filename = generatorKml.GenerateFileName(rule);


            var colour = Rules.GetRules().ToList().IndexOf(rule);
            var kml = generatorKml.Generate(result, filename, colour);
            SendFileAsync(message.Chat.Id, kml, filename);

            WriteLog(message.Chat.Id, filename);
        }

        void WriteLog(long id, string filename)
        {
            //var logfile = "log" + DateTime.Now.Year % 100 + DateTime.Now.Month + ".txt";
            var logfile = "log.txt";
            var split = filename.Split('_');

            using (StreamWriter wr = new StreamWriter(Config.LogFolder + logfile, true))
            {
                wr.WriteLine("{2} {3}   {0,-17}{1}", id, split[0], split[1], split[2]);
            }
        }

        public async static Task SendFileAsync(long chatId, string kml, string filename)
        {
            var url = String.Format("https://api.telegram.org/bot{0}/sendDocument", Config.Token);

            using (var form = new MultipartFormDataContent())
            {
                using (Stream kmlStream = GenerateStreamFromString(kml))
                {
                    form.Add(new StringContent(chatId.ToString(), Encoding.UTF8), "chat_id");
                    form.Add(new StreamContent(kmlStream), "document", filename);

                    using (var client = new HttpClient())
                    {
                        await client.PostAsync(url, form);
                    }
                }
            }
        }

        static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
