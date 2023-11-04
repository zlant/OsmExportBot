using OsmExportBot.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        static Semaphore semaphore = new Semaphore(1, 1);

        static int offset = 0;

        static List<Command> commandsWithKeyWord = new List<Command>() {
            new StartCommand(),
            new ShowCommand(),
            new RulesCommand(),
            new SetCommand(),
            new CreateCommand()
        };

        static LocationCommand locationCommand = new LocationCommand();

        public static TelegramBotClient Client { get; set; } = 
            new TelegramBotClient(Config.Token);

        public static async Task<Update[]> GetUpdates()
        {
            return await Client.GetUpdatesAsync(offset);
        }

        public async static Task ProcessingUpdates(Update[] updates)
        {
            foreach (var update in updates)
            {
                await ProcessingUpdate(update);
            }
        }

        public async static Task ProcessingUpdate(Update update)
        {
            if (update.Id >= offset)
                offset = update.Id + 1;
            try
            {
                if (update.Message.Type == MessageType.Text)
                {
                    var command = commandsWithKeyWord
                        .FirstOrDefault(x => update.Message.Text.StartsWith("/" + x.Name));

                    if (command != null)
                    {
                        await command.Excecute(update.Message, Client);
                    }
                    else if (UserState.NewRule.ContainsKey(update.Message.Chat.Id))
                    {
                        if (Rules.NewRule(update))
                            await Client.SendTextMessageAsync(update.Message.Chat.Id, "Новое правило создано.");
                    }
                    else if (update.Message.Text.StartsWith("/"))
                    {
                        var rule = update.Message.Text.Split(' ').First().Trim().TrimStart('/').ToLower();

                        if (Rules.GetRules().Contains(rule))
                        {
                            UserState.CurrentRule[update.Message.Chat.Id] = rule;
                            await Client.SendTextMessageAsync(update.Message.Chat.Id, "Теперь отправьте местоположение.");
                        }
                        else
                        {
                            await Client.SendTextMessageAsync(update.Message.Chat.Id, "Такого правила не существует.");
                        }
                    }
                }
                else if (update.Message.Type == MessageType.Location)
                {
                    await locationCommand.Excecute(update.Message, Client);
                }
            }
            catch (Exception ex)
            {
                await Client.SendTextMessageAsync(update.Message.Chat.Id, "Что-то пошло не так, попробуйте еще раз.");
            }
        }

        public static async void Run(object obj)
        {
            semaphore.WaitOne();
            try
            {
                await ProcessingUpdates(await GetUpdates());
            }
            catch (Exception ex)
            {
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
