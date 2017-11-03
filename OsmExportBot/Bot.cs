using OsmExportBot.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace OsmExportBot
{
    public static class Bot
    {
        public static string status { get; set; } = "Off";
        static Semaphore semaphore = new Semaphore(1, 1);

        static int offset = 0;

        static TelegramBotClient bot = new TelegramBotClient(Config.Token);

        static List<Command> commandsWithKeyWord = new List<Command>() {
            new StartCommand(),
            new ShowCommand(),
            new RulesCommand(),
            new SetCommand(),
            new CreateCommand()
        };

        static LocationCommand locationCommand = new LocationCommand();

        public static async Task<Update[]> GetUpdates()
        {
            return await bot.GetUpdatesAsync(offset);
        }

        public static void ProcessingUpdates(Update[] updates)
        {
            foreach (var update in updates)
            {
                if (update.Id >= offset)
                    offset = update.Id + 1;
                try
                {
                    if (update.Message.Type == MessageType.TextMessage)
                    {
                        WriteLog(update.Message.Chat.Id, update.Message.Text);

                        var command = commandsWithKeyWord
                            .FirstOrDefault(x => update.Message.Text.StartsWith("/" + x.Name));

                        if (command != null)
                        {
                            command.Excecute(update.Message, bot);
                        }
                        else if (UserState.NewRule.ContainsKey(update.Message.Chat.Id))
                        {
                            if (Rules.NewRule(update))
                                bot.SendTextMessageAsync(update.Message.Chat.Id, "Новое правило создано.");
                        }
                        else if (update.Message.Text.StartsWith("/"))
                        {
                            var rule = update.Message.Text.Split(' ').First().Trim().TrimStart('/').ToLower();

                            if (Rules.GetRules().Contains(rule))
                            {
                                UserState.CurrentRule[update.Message.Chat.Id] = rule;
                                bot.SendTextMessageAsync(update.Message.Chat.Id, "Теперь отправьте местоположение.");
                            }
                            else
                            {
                                bot.SendTextMessageAsync(update.Message.Chat.Id, "Такого правила не существует.");
                            }
                        }
                    }
                    else if (update.Message.Type == MessageType.LocationMessage)
                    {
                        locationCommand.Excecute(update.Message, bot);
                    }
                }
                catch (Exception ex)
                {
                    bot.SendTextMessageAsync(update.Message.Chat.Id, "Что-то пошло не так, попробуйте еще раз.");
                    WriteError(ex);
                }
            }
        }

        static void WriteLog(long id, string message)
        {
            var logfile = "log_full.txt";

            using (StreamWriter wr = new StreamWriter(Config.LogFolder + logfile, true))
            {
                wr.WriteLine("{2}   {0,-17}{1}", id, message, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
            }
        }

        static void WriteError(Exception ex)
        {
            var logfile = "log_error.txt";

            using (StreamWriter wr = new StreamWriter(Config.LogFolder + logfile, true))
            {
                wr.WriteLine("{0}: {1}\r\n{2}",
                    DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"),
                    ex.Message,
                    ex.StackTrace);
            }
        }

        public static async void Run(object obj)
        {
            semaphore.WaitOne();
            try
            {
                ProcessingUpdates(await GetUpdates());
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
