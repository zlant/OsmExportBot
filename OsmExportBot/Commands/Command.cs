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
    public abstract class Command
    {
        public abstract string Name { get; set; }
        public abstract MessageType Type { get; set; }

        public abstract void Excecute(Message message, TelegramBotClient bot);

        public bool Contains(string text)
        {
            return text.Contains(this.Name);
        }
    }
}
