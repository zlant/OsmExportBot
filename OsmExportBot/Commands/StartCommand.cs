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
    public class StartCommand : Command
    {
        public override string Name { get; set; } = "start";

        public override MessageType Type { get; set; } = MessageType.Text;

        public async override Task Excecute(Message message, TelegramBotClient bot)
        {
            await bot.SendTextMessageAsync(message.Chat.Id, Config.StartText, parseMode: ParseMode.Markdown);
        }
    }
}
