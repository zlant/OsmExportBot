using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace OsmExportBot.Commands
{
    public class SetCommand : Command
    {
        public override string Name { get; set; } = "set";

        public override MessageType Type { get; set; } = MessageType.TextMessage;

        public override void Excecute(Message message, TelegramBotClient bot)
        {
            var words = message.Text.Split(' ');
            if (words.Length != 2)
            {
                bot.SendTextMessage(message.Chat.Id, "Неправильный формат команды. Пример: `/set name`", parseMode: ParseMode.Markdown);
                return;
            }
            var rule = words[1].Trim().ToLower();
            if (!Regex.IsMatch(rule, @"^[A-Za-z0-9]+$"))
            {
                bot.SendTextMessage(message.Chat.Id, "Название может содержать только буквы латинского алфавита и цифры.");
                return;
            }
            if (Rules.GetRules().Contains(rule))
            {
                bot.SendTextMessage(message.Chat.Id, "Правило с таким именем уже существует, придумайте другое название.");
                return;
            }

            UserState.NewRule[message.Chat.Id] = rule;
            bot.SendTextMessage(message.Chat.Id, "Теперь отправьте текст overpass запроса.");
        }
    }
}
