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

            if (UserState.CurrentRule.ContainsKey(message.Chat.Id))
                UserState.CurrentRule.TryRemove(message.Chat.Id, out rule);

            var query = overpass.BuildQuery(rule, message.Location.Latitude, message.Location.Longitude);

            var result = overpass.RunQuery(query);
            var fileName = generatorKml.GenerateFileName(rule);


            var colour = Rules.GetRules().ToList().IndexOf(rule);
            var fileContent = generatorKml.Generate(result, fileName, colour);
            SendFile(message.Chat.Id, fileContent, fileName);

            Console.WriteLine(fileName);
            WriteLog(message.Chat.Id, fileName);
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
