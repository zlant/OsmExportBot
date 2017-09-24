using OsmExportBot.Commands;
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

namespace OsmExportBot
{
    public static class Bot
    {
        static int offset = 0;

        public static string status { get; set; } = "Off";
        static object locker = new object();

        static TelegramBotClient bot = new TelegramBotClient(Config.Token);

        static List<Command> commandsWithKeyWord = new List<Command>() {
            new StartCommand(),
            new ShowCommand(),
            new RulesCommand(),
            new SetCommand()
        };

        static LocationCommand locationCommand = new LocationCommand();

        public static async Task GetUpdates()
        {
            var updates = await bot.GetUpdates(offset);

            foreach (var update in updates)
            {
                if (update.Id >= offset)
                    offset = update.Id + 1;

                if (update.Message.Type == MessageType.TextMessage)
                {
                    var command = commandsWithKeyWord.FirstOrDefault(x => update.Message.Text.Contains(x.Name));

                    if (command != null)
                        command.Excecute(update.Message, bot);
                    else if (UserState.NewRule.ContainsKey(update.Message.Chat.Id))
                    {
                        if (Rules.NewRule(update))
                            bot.SendTextMessage(update.Message.Chat.Id, "Новое правило создано.");
                    }
                    else if (update.Message.Text.StartsWith("/"))
                    {
                        var rule = update.Message.Text.Split(' ').First().Trim().TrimStart('/').ToLower();

                        if (Rules.GetRules().Contains(rule))
                        {
                            UserState.CurrentRule[update.Message.Chat.Id] = rule;
                            bot.SendTextMessage(update.Message.Chat.Id, "Теперь отправьте местоположение.");
                        }
                        else
                        {
                            bot.SendTextMessage(update.Message.Chat.Id, "Такого правила не существует.");
                        }
                    }

                }
                else if (update.Message.Type == MessageType.LocationMessage)
                {
                    locationCommand.Excecute(update.Message, bot);
                }
            }
        }

        public static void Run(object obj)
        {
            lock (locker)
            {
                GetUpdates();
            }
        }

    }
}
