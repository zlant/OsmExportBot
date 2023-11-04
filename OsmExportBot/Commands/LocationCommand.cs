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
using Telegram.Bot.Types.InputFiles;

namespace OsmExportBot.Commands
{
    public class LocationCommand : Command
    {
        public override string Name { get; set; }

        public override MessageType Type { get; set; } = MessageType.Location;

        private Overpass overpass = new Overpass();

        private GeneratorKml generatorKml = new GeneratorKml();

        public async override Task Excecute(Message message, TelegramBotClient bot)
        {
            string rule = "nolvl";

            if (UserState.CurrentRule.ContainsKey(message.Chat.Id))
                UserState.CurrentRule.TryRemove(message.Chat.Id, out rule);

            var query = new Query { RuleName = rule };
            overpass.BuildQuery(query, message.Location.Latitude, message.Location.Longitude);

            string fileName = generatorKml.GenerateFileName(rule);
            overpass.RunQuery(query);
            string fileContent = generatorKml.Generate(query.Response, fileName);

            var file = new InputOnlineFile(new MemoryStream(Encoding.UTF8.GetBytes(fileContent)), fileName);
            await bot.SendDocumentAsync(message.Chat.Id, file);

            Console.WriteLine(fileName);
        }
    }
}
