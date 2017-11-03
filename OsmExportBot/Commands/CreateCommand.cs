using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace OsmExportBot.Commands
{
    public class CreateCommand : Command
    {
        public override string Name { get; set; } = "create";

        public override MessageType Type { get; set; } = MessageType.TextMessage;

        public override void Excecute(Message message, TelegramBotClient bot)
        {
            bot.SendTextMessageAsync(message.Chat.Id, Config.CreateInfoText, parseMode: ParseMode.Markdown);
        }
    }
}
