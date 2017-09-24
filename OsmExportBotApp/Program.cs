using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace OsmExportBotApp
{
    class Program
    {
        static Timer timer;
        static long interval = 1000;

        static void Main(string[] args)
        {
            OsmExportBot.Config.Token = "TOKEN";
            OsmExportBot.Config.RulesFolder = "Queries\\";
            timer = new Timer(new TimerCallback(OsmExportBot.Bot.Run), null, 0, interval);
            //OsmExportBot.Bot.Run( null );
            Console.ReadLine();
        }
    }
}
