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

        public override MessageType Type { get; set; } = MessageType.Text;

        public async override Task Excecute(Message message, TelegramBotClient bot)
        {
            var words = message.Text.Split(' ');
            if (words.Length != 2)
            {
                await bot.SendTextMessageAsync(message.Chat.Id, "Неправильный формат команды. Пример: `/show nolvl`", parseMode: ParseMode.Markdown);
                return;
            }
            var name = words[1].ToLower();
            var query = Queries.Queries.ResourceManager.GetString(name);
            await bot.SendTextMessageAsync(message.Chat.Id, query);
            return;
        }
    }
}
