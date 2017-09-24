using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace OsmExportBot.Commands
{
    public class ShowCommand : Command
    {
        public override string Name { get; set; } = "show";

        public override MessageType Type { get; set; } = MessageType.TextMessage;

        public override void Excecute(Message message, TelegramBotClient bot)
        {
            var words = message.Text.Split(' ');
            if (words.Length != 2)
            {
                bot.SendTextMessage(message.Chat.Id, "Неправильный формат команды. Пример: `/show nolvl`", parseMode: ParseMode.Markdown);
                return;
            }
            var name = words[1].ToLower();
            string path = Config.RulesFolder + name + ".txt";
            using (var rd = new StreamReader(path))
                bot.SendTextMessage(message.Chat.Id, rd.ReadToEnd());
            return;
        }
    }
}
