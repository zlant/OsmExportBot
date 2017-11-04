using OsmExportBot.DataSource;
using OsmExportBot.Primitives;
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

        private Overpass overpass = new Overpass();

        private GeneratorKml generatorKml = new GeneratorKml();

        public override void Excecute(Message message, TelegramBotClient bot)
        {
            string rule = "nolvl";

            if (UserState.CurrentRule.ContainsKey(message.Chat.Id))
                UserState.CurrentRule.TryRemove(message.Chat.Id, out rule);

            var query = new Query { RuleName = rule };
            overpass.BuildQuery(query, message.Location.Latitude, message.Location.Longitude);

            string fileName = generatorKml.GenerateFileName(rule);
            overpass.RunQuery(query);
            string fileContent = generatorKml.Generate(query.Response, fileName);
            
            SendFile(message.Chat.Id, fileContent, fileName);

            Console.WriteLine(fileName);
            WriteLog(message.Chat.Id, rule);
        }

        void WriteLog(long id, string ruleName)
        {
            //var logfile = "log" + DateTime.Now.Year % 100 + DateTime.Now.Month + ".txt";
            var logfile = "log.txt";

            using (StreamWriter wr = new StreamWriter(Config.LogFolder + logfile, true))
            {
                wr.WriteLine("{2} {3}   {0,-17}{1}",
                    id,
                    ruleName,
                    DateTime.Now.ToString("yyyy-MM-dd"),
                    DateTime.Now.ToString("HH-mm-ss"));
            }
        }

        public static async void SendFile(long chatId, string fileContent, string fileName)
        {
            var url = String.Format("https://api.telegram.org/bot{0}/sendDocument", Config.Token);

            using (var form = new MultipartFormDataContent())
            {
                using (Stream fileContentStream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent)))
                {
                    form.Add(new StringContent(chatId.ToString(), Encoding.UTF8), "chat_id");
                    form.Add(new StreamContent(fileContentStream), "document", fileName);

                    using (var client = new HttpClient())
                    {
                        await client.PostAsync(url, form);
                    }
                }
            }
        }
    }
}
