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
    class RulesCommand : Command
    {
        public override string Name { get; set; } = "rules";

        public override MessageType Type { get; set; } = MessageType.TextMessage;

        public override void Excecute(Message message, TelegramBotClient bot)
        {
            string msg = "Нажмите на нужный набор:\r\n";
            string commands = Rules.GetRulesCommands();
            bot.SendTextMessage(message.Chat.Id, msg + commands);
            return;
        }
    }
}
