using Newtonsoft.Json;
using SharpKml.Base;
using SharpKml.Dom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace OsmExporterBot
{
    class Program
    {
        static Timer timer;
        static long interval = 5000;

        static void Main( string[] args )
        {
            OsmToKmlBot.Config.Token = "TOKEN";
            OsmToKmlBot.Config.RulesFolder = "Queries\\";
            timer = new Timer( new TimerCallback( OsmToKmlBot.Bot.Run ), null, 0, interval );

            Console.ReadLine();
        }
    }
}
