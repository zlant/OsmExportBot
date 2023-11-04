using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
            var rules = Queries.Queries.ResourceManager
                .GetResourceSet(CultureInfo.CurrentCulture, true, true)!
                .Cast<DictionaryEntry>()
                .Where(x => x.Value is string)
                .Select(x => x.Key.ToString())
                .ToList();
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
            throw new NotImplementedException();
        }

        public static string GetRule(string name)
        {
            string query = Queries.Queries.ResourceManager.GetString(name)!;
            return query;
        }

        public static int IndexOfRule(string name)
        {
            return GetRules().ToList().IndexOf(name);
        }
    }
}
