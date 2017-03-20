using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace OsmToKmlBot
{
    public class Bot
    {
        static int offset = 0;
        public static int countq = 0;

        public static string status = "Off";
        static object locker = new object();

        static TelegramBotClient bot = new TelegramBotClient( Config.Token );
        static List<ChatRule> currentRulesForChats = new List<ChatRule>();
        static List<ChatRule> newRuleFromChats = new List<ChatRule>();

        public static async Task GetUpdates()
        {
            status = "GetUpdates";
            var updates = BotQueries.GetUpdates( offset );

#if DEBUG
            Console.Clear();
            Console.WriteLine( countq );
#endif
            foreach ( var update in updates )
            {
                if ( update.Id >= offset )
                    offset = update.Id + 1;

                if ( update.Message.Type == MessageType.TextMessage )
                {
                    RequestForText( update );
                    continue;
                }

                if ( update.Message.Type != MessageType.LocationMessage )
                    continue;

#if DEBUG
                Console.WriteLine( String.Format( "{0} | {1}",
                    update.Message.Location.Latitude,
                    update.Message.Location.Longitude ) );
#endif

                string rule = "nolvl";
                if ( currentRulesForChats.Exists( x => x.ChatId == update.Message.Chat.Id ) )
                {
                    rule = currentRulesForChats.Find( x => x.ChatId == update.Message.Chat.Id ).RuleName;
                    currentRulesForChats.RemoveAll( x => x.ChatId == update.Message.Chat.Id );
                }

                var thisQuery = BotQueries.BuildQuery( rule, update.Message.Location.Latitude, update.Message.Location.Longitude );

                status = "SendQuery";
                var result = Overpass.RunQuery( thisQuery );//await RunQueryAsync( thisQuery );
                var filename = GenerateFileName( rule );


                status = "SendFile";
                var kml = GeneratorKml.GenerateKml( result, filename );
                await BotQueries.SendFileAsync( update.Message.Chat.Id, kml, filename + ".kml" );

                WriteLog( update.Message.Chat.Id, filename );

                countq++;
            }
            status = "End";
        }

        static void WriteLog( long id, string filename )
        {
            //var logfile = "log" + DateTime.Now.Year % 100 + DateTime.Now.Month + ".txt";
            var logfile = "log.txt";
            var split = filename.Split( '_' );

            using ( StreamWriter wr = new StreamWriter( Config.LogFolder + logfile, true ) )
            {
                wr.WriteLine( "{2} {3}   {0,-17}{1}", id, split[ 0 ], split[ 1 ], split[ 2 ] );
            }
        }

        static void RequestForText( Update update )
        {
            if ( update.Message.Text == "/start" )
            {
                bot.SendTextMessage( update.Message.Chat.Id, Config.StartText, parseMode: ParseMode.Markdown );
                return;
            }
            else if ( update.Message.Text.StartsWith( "/show" ) )
            {
                var words = update.Message.Text.Split( ' ' );
                if ( words.Length != 2 )
                {
                    bot.SendTextMessage( update.Message.Chat.Id, "Неправильный формат команды. Пример: `/show nolvl`", parseMode: ParseMode.Markdown );
                    return;
                }
                var name = words[ 1 ].ToLower();
                string path = Config.RulesFolder + name + ".txt";
                using ( var rd = new StreamReader( path ) )
                    bot.SendTextMessage( update.Message.Chat.Id, rd.ReadToEnd() );
                return;
            }
            else if ( update.Message.Text == "/rules" )
            {
                string message = "Нажмите на нужный набор:\r\n";
                string commands = GetRulesCommands();
                bot.SendTextMessage( update.Message.Chat.Id, message + commands );
                return;
            }
            else if ( update.Message.Text.StartsWith( "/set" ) )
            {
                var words = update.Message.Text.Split( ' ' );
                if ( words.Length != 2 )
                {
                    bot.SendTextMessage( update.Message.Chat.Id, "Неправильный формат команды. Пример: `/set name`", parseMode: ParseMode.Markdown );
                    return;
                }
                var rule = words[ 1 ].Trim().ToLower();
                if ( !Regex.IsMatch( rule, @"^[A-Za-z0-9]+$" ) )
                {
                    bot.SendTextMessage( update.Message.Chat.Id, "Название может содержать только буквы латинского алфавита и цифры." );
                    return;
                }
                if ( GetRules().Contains( rule ) )
                {
                    bot.SendTextMessage( update.Message.Chat.Id, "Правило с таким именем уже существует, придумайте другое название." );
                    return;
                }
                newRuleFromChats.Add( new ChatRule {
                    ChatId = update.Message.Chat.Id,
                    RuleName = rule
                } );
                bot.SendTextMessage( update.Message.Chat.Id, "Теперь отправьте текст overpass запроса." );
            }
            else if ( newRuleFromChats.Exists( x => x.ChatId == update.Message.Chat.Id ) )
            {
                var name = newRuleFromChats.Find( x => x.ChatId == update.Message.Chat.Id ).RuleName;
                newRuleFromChats.RemoveAll( x => x.ChatId == update.Message.Chat.Id );

                using ( StreamWriter wr = new StreamWriter( Config.RulesFolder + name + ".txt" ) )
                {
                    wr.Write( update.Message.Text );
                }

                bot.SendTextMessage( update.Message.Chat.Id, "Новое правило создано. /" + name );
                return;
            }
            else if ( update.Message.Text.StartsWith( "/" ) )
            {
                var rule = update.Message.Text.Split( ' ' )[ 0 ].Trim().TrimStart( '/' ).ToLower();

                if ( !GetRules().Contains( rule ) )
                {
                    bot.SendTextMessage( update.Message.Chat.Id, "Такого правила не существует." );
                    return;
                }

                currentRulesForChats.RemoveAll( x => x.ChatId == update.Message.Chat.Id );
                currentRulesForChats.Add( new ChatRule {
                    ChatId = update.Message.Chat.Id,
                    RuleName = rule
                } );
                bot.SendTextMessage( update.Message.Chat.Id, "Теперь отправьте местоположение." );

                return;
            }
        }

        private static IEnumerable<string> GetRules()
        {
            var rules = Directory.GetFiles( Config.RulesFolder )
                                .Select( x => x.Split( new char[] { '\\', '/' } ).Last() )
                                .Select( x => x.Split( '.' )[ 0 ] );
            return rules;
        }

        private static string GetRulesCommands()
        {
            var rules = Directory.GetFiles( Config.RulesFolder )
                                .Select( x => x.Split( new char[] { '\\', '/' } ).Last() )
                                .Select( x => x.Split( '.' )[ 0 ] )
                                .Select( x => "/" + x );
            var text = string.Join( "\r\n", rules );
            return text;
        }

        public static void Run( object obj )
        {
            //if ( !running )
            lock ( locker )
            {
                GetUpdates();
            }
        }

        static string GenerateFileName( string rule )
        {
            return string.Format( "{6}_{0}-{1:00}-{2:00}_{3:00}-{4:00}-{5:00}",
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                DateTime.Now.Hour,
                DateTime.Now.Minute,
                DateTime.Now.Second,
                rule
                );
        }
    }
}