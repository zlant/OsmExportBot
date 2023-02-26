using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace OsmExportBot
{
    static class Rules
    {
        public static IEnumerable<string> GetRules()
        {
            var rules = Directory.GetFiles(Config.RulesFolder)
                .Select(x => x.Split(Path.DirectorySeparatorChar).Last())
                .Select(x => x.Split('.').First());
            return rules;
        }

        public static string GetRulesCommands()
        {
            var rules = GetRules().Select(x => "/" + x);
            var text = string.Join("\r\n", rules);
            return text;
        }

        public static bool NewRule(Update update)
        {
            string name;
            UserState.NewRule.TryRemove(update.Message.Chat.Id, out name);

            using (StreamWriter wr = new StreamWriter(Config.RulesFolder + name + ".overpassql"))
            {
                wr.Write(update.Message.Text);
            }

            return true;
        }

        public static string GetRule(string name)
        {
            string query;
            string path = Config.RulesFolder + name + ".overpassql";

            using (var rd = new StreamReader(path))
                query = rd.ReadToEnd();

            return query;
        }

        public static int IndexOfRule(string name)
        {
            return GetRules().ToList().IndexOf(name);
        }
    }
}
